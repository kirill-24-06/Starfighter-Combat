using System.Collections;
using UnityEngine;

public class BossDefenceDrone : Enemy
{
    [SerializeField] private BossDefenceDroneData _data;

    private Patrol _mover;
    private AudioSource _audioPlayer;
    private IAttacker _attacker;
    private WaitForSeconds _awaitTime;

    private Transform _target;

    public BossDefenceDrone InitialiseByBoss()
    {
        Initialise();

        return this;
    }

    protected override void Initialise()
    {
        _mover = new Patrol(transform, _data);
        _target = EntryPoint.Instance.Player.transform;
        _attacker = new EnemyAttacker(this, _data);
        _audioPlayer = EntryPoint.Instance.GlobalSoundFX;
        _awaitTime = new WaitForSeconds(1);

        _mover.Arrival += OnArrival;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _health = _data.Health;
    }

    public void Handle()
    {
        Move();
        Attack();
    }

    protected override void Move()
    {
        _mover.Move(_data.Speed);
        DeactivateOutOfBounds(_data.DisableBorders);
    }

    private void Attack()
    {
        _mover.LookInTargetDirection(_target.position);
        _attacker.Fire(_data.EnemyMissile.gameObject);
    }

    protected override void Collide()
    {
        if (!EntryPoint.Instance.Player.IsInvunerable && !EntryPoint.Instance.Player.IsDroneActive)
            _events.PlayerDamaged?.Invoke(GlobalConstants.CollisionDamage);

        else if (!EntryPoint.Instance.Player.IsInvunerable && EntryPoint.Instance.Player.IsDroneActive)
            _events.DroneDestroyed?.Invoke();

        TakeDamage(GlobalConstants.CollisionDamage*5);
    }

    private void OnArrival() => StartCoroutine(Patrol());

    private IEnumerator Patrol()
    {
        while (gameObject.activeInHierarchy)
        {
            if (!_mover.IsMoving)
            {
                yield return _awaitTime;
                _mover.SetNewDirection();
            }
            yield return null;
        }
    }


    protected override void Disable()
    {
        Instantiate(_data.Explosion, transform.position, _data.Explosion.transform.rotation);

        ObjectPoolManager.ReturnObjectToPool(gameObject);

        _events.AddScore?.Invoke(_data.Score);

        _audioPlayer.PlayOneShot(_data.ExplosionSound, _data.ExplosionSoundVolume);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _attacker.Reset();
        _mover.Reset();
        _mover.NewMovePoints();
        StopAllCoroutines();
    }
}