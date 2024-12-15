using UnityEngine;

public class Interceptor : Enemy
{
    [SerializeField] private InterceptorData _data;

    private MovementControl _mover;

    private IAttacker _attackHandler;
    private IResetable _attacker;
    private Transform _target;

    private Timer _liveTimer;

    protected override void Awake()
    {
        base.Awake();
        Initialise();

        PoolRootMap.SetParrentObject(_gameObject, GlobalConstants.PoolTypesByTag[_data.Tag]);
    }

    private void Start()
    {
        var collider = GetComponent<Collider2D>();
        EntryPoint.Instance.CollisionMap.Register(collider, this);
        EntryPoint.Instance.CollisionMap.RegisterNukeInteractable(collider, this);
        EntryPoint.Instance.CollisionMap.RegisterMissileTarget(_transform);
    }

    protected override void Initialise()
    {
        _mover = new MovementControl(transform, _data);
        _mover.Arrival += OnArrival;

        var attacker = new EnemyAdvancedAttacker(this, _data, _data.ShootsBeforeRetreat);
        _attackHandler = attacker;
        _attacker = attacker;
        _target = EntryPoint.Instance.Player.transform;
        attacker.AttackRunComplete += OnAttackRunComplete;

        var enemyHealth = new EnemyHealthHandler(_data.Health);
        _damageHandler = enemyHealth;
        _health = enemyHealth;
        enemyHealth.Dead += OnDead;

        _liveTimer = new Timer(this);
        _liveTimer.TimeIsOver += _mover.Disengage;
    }

    private void OnEnable()
    {
        _attacker.Reset();
        _mover.Reset();
        _health.Reset();
        _isInPool = false;
    }
   
    private void FixedUpdate() => Attack();

    private void Update() => Move();

    protected override void Move() => _mover.Move(_data.Speed);

    protected override void Collide()
    {
        if (!EntryPoint.Instance.Player.IsInvunerable && !EntryPoint.Instance.Player.IsDroneActive)
            _events.PlayerDamaged?.Invoke(GlobalConstants.CollisionDamage);

        else if (!EntryPoint.Instance.Player.IsInvunerable && EntryPoint.Instance.Player.IsDroneActive)
            _events.DroneDestroyed?.Invoke();

        _damageHandler.TakeDamage(GlobalConstants.CollisionDamage * _data.Health);
    }

    private void Attack()
    {
        if (!_mover.IsMoving && _target.gameObject.activeInHierarchy)
        {
            _mover.LookInTargetDirection(_target.position);
            _attackHandler.Fire();
        }
    }

    private void OnAttackRunComplete() => _mover.SetNewDirection();

    private void OnArrival()
    {
        _liveTimer.SetTimer(_data.LiveTime);
        _liveTimer.StartTimer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isInPool) return;
        _isInPool = true;

        _events.EnemyDestroyed?.Invoke(_data.EnemyStrenght);
        ObjectPool.Release(_gameObject);
    }

    protected override void OnDead()
    {
        if (_isInPool) return;
        _isInPool = true;

        ObjectPool.Get(_data.Explosion, _transform.position,
         _data.Explosion.transform.rotation);

        _events.AddScore?.Invoke(_data.Score);
        _events.EnemyDestroyed?.Invoke(_data.EnemyStrenght);

        _soundPlayer.PlayOneShot(_data.ExplosionSound, _data.ExplosionSoundVolume);

        _liveTimer.StopTimer();

        ObjectPool.Release(_gameObject);
    }
}