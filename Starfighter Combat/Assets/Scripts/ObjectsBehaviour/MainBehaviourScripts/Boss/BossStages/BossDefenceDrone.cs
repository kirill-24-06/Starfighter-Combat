using System.Collections;
using UnityEngine;

public class BossDefenceDrone : Enemy
{
    [SerializeField] private BossDefenceDroneData _data;

    private Patrol _mover;

    private IAttacker _attackHandler;
    private IResetable _attacker;
    private WaitForSeconds _awaitTime;
    private Transform _target;

    public BossDefenceDrone InitialiseByBoss()
    {
        Initialise();

        return this;
    }

    protected override void Awake()
    {
        base.Awake();
        PoolMap.SetParrentObject(GlobalConstants.PoolTypesByTag[_data.Tag]);
    }

    protected override void Initialise()
    {
        _mover = new Patrol(transform, _data);
        _mover.Arrival += OnArrival;

        var attacker = new EnemyAttacker(this, _data);
        _attacker = attacker;
        _attackHandler = attacker;
        _target = EntryPoint.Instance.Player.transform;

        var enemyHealth = new EnemyHealthHandler(_data.Health);
        _damageHandler = enemyHealth;
        _health = enemyHealth;
        enemyHealth.Dead += OnDead;

        _awaitTime = new WaitForSeconds(1);
    }

    private void Start()
    {
        var collider = GetComponent<Collider2D>();
        EntryPoint.Instance.CollisionMap.Register(collider, this);
        EntryPoint.Instance.CollisionMap.RegisterNukeInteractable(collider, this);
        EntryPoint.Instance.CollisionMap.RegisterMissileTarget(_transform);
    }

    private void OnEnable()
    {
        _health?.Reset();
        _attacker?.Reset();
        _mover?.Reset();
        _mover?.NewMovePoints();
    }

    public void Handle()
    {
        Move();
        Attack();
    }

    protected override void Move() => _mover.Move(_data.Speed);

    private void Attack()
    {
        if (_target.gameObject.activeInHierarchy)
        {
            _mover.LookInTargetDirection(_target.position);
            _attackHandler.Fire();
        }
    }

    public override void GetDamagedByNuke() => _damageHandler.TakeDamage((int)(GlobalConstants.NukeDamage * _data.NukeResist));

    protected override void Collide()
    {
        if (!EntryPoint.Instance.Player.IsInvunerable && !EntryPoint.Instance.Player.IsDroneActive)
            _events.PlayerDamaged?.Invoke(GlobalConstants.CollisionDamage);

        else if (!EntryPoint.Instance.Player.IsInvunerable && EntryPoint.Instance.Player.IsDroneActive)
            _events.DroneDestroyed?.Invoke();

        _damageHandler.TakeDamage(GlobalConstants.CollisionDamage);
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

    private void OnTriggerEnter2D(Collider2D collision) => ObjectPool.Release(_gameObject);

    protected override void OnDead()
    {
        ObjectPool.Get(_data.Explosion, _transform.position,
           _data.Explosion.transform.rotation);

        _events.AddScore?.Invoke(_data.Score);

        _soundPlayer.PlayOneShot(_data.ExplosionSound, _data.ExplosionSoundVolume);

        StopCoroutine(Patrol());

        ObjectPool.Release(_gameObject);
    }
}