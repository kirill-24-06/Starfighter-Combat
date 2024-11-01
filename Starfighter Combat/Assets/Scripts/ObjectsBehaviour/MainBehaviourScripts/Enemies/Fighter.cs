using UnityEngine;

public class Fighter : Enemy
{
    [SerializeField] private FighterData _data;

    private IMover _mover;

    private IAttacker _attackHandler;
    private IResetable _attacker;

    protected override void Awake()
    {
        base.Awake();
        PoolMap.SetParrentObject(GlobalConstants.PoolTypesByTag[_data.Tag]);
    }

    private void Start()
    {
        Initialise();

        var collider = GetComponent<Collider2D>();
        EntryPoint.Instance.CollisionMap.Register(collider, this);
        EntryPoint.Instance.CollisionMap.RegisterNukeInteractable(collider, this);
        EntryPoint.Instance.CollisionMap.RegisterMissileTarget(_transform);
    }

    protected override void Initialise()
    {
        _mover = new Mover(_transform);

        var attacker = new EnemyAttacker(this, _data);
        _attackHandler = attacker;
        _attacker = attacker;

        var enemyHealth = new EnemyHealthHandler(_data.Health);
        _damageHandler = enemyHealth;
        _health = enemyHealth;
        enemyHealth.Dead += OnDead;
    }

    private void OnEnable()
    {
        _health?.Reset();
        _attacker?.Reset();
    }

    private void FixedUpdate() => _attackHandler.Fire();

    private void Update() => Move();

    protected override void Move() => _mover.Move(Vector2.up, _data.Speed);

    protected override void Collide()
    {
        if (!EntryPoint.Instance.Player.IsInvunerable && !EntryPoint.Instance.Player.IsDroneActive)
            _events.PlayerDamaged?.Invoke(GlobalConstants.CollisionDamage);

        else if (!EntryPoint.Instance.Player.IsInvunerable && EntryPoint.Instance.Player.IsDroneActive)
            _events.DroneDestroyed?.Invoke();

        _damageHandler.TakeDamage(GlobalConstants.CollisionDamage * _data.Health);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _events.EnemyDestroyed?.Invoke(_data.EnemyStrenght);
        ObjectPool.Release(gameObject);
    }

    protected override void OnDead()
    {
        ObjectPool.Get(_data.Explosion, transform.position,
            _data.Explosion.transform.rotation);

        _events.AddScore?.Invoke(_data.Score);
        _events.EnemyDestroyed?.Invoke(_data.EnemyStrenght);

        _soundPlayer.PlayOneShot(_data.ExplosionSound, _data.ExplosionSoundVolume);

        ObjectPool.Release(gameObject);
    }
}