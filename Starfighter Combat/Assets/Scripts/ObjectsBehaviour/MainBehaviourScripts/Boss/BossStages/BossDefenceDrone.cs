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

    private void Start()
    {
        var collider = GetComponent<Collider2D>();
        EntryPoint.Instance.CollisionMap.Register(collider, this);
        EntryPoint.Instance.CollisionMap.RegisterNukeInteractable(collider, this);
        EntryPoint.Instance.MissileTargets.AddEnemy(transform);
    }

    private void OnEnable()
    {
        _health = _data.Health;
    }

    public void Handle()
    {
        Attack();
    }

    protected override void Move()
    {
        _mover.Move(_data.Speed);
        DeactivateOutOfBounds(_data.DisableBorders);
    }

    private void Attack()
    {
        if (_target.gameObject.activeInHierarchy)
        {
            _mover.LookInTargetDirection(_target.position);
            _attacker.Fire(_data.EnemyMissile.gameObject);
        }
    }

    protected override void Collide()
    {
        if (!EntryPoint.Instance.Player.IsInvunerable && !EntryPoint.Instance.Player.IsDroneActive)
            _events.PlayerDamaged?.Invoke(GlobalConstants.CollisionDamage);

        else if (!EntryPoint.Instance.Player.IsInvunerable && EntryPoint.Instance.Player.IsDroneActive)
            _events.DroneDestroyed?.Invoke();

        TakeDamage(GlobalConstants.CollisionDamage * 5);
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
        ObjectPoolManager.SpawnObject(_data.Explosion, transform.position,
            _data.Explosion.transform.rotation, ObjectPoolManager.PoolType.ParticleSystem);

        ObjectPoolManager.ReturnObjectToPool(gameObject);

        _events.AddScore?.Invoke(_data.Score);

        _audioPlayer.PlayOneShot(_data.ExplosionSound, _data.ExplosionSoundVolume);
    }

    private void OnDisable()
    {
        _attacker?.Reset();
        _mover?.Reset();
        _mover?.NewMovePoints();
        StopAllCoroutines();
    }
}