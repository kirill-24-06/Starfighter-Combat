using UnityEngine;

public class Asteroid : Enemy
{
    [SerializeField] private AsteroidData _data;

    private IMover _mover;

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

    private void OnEnable() => _health?.Reset();

    protected override void Initialise()
    {
        _mover = new Mover(_transform);

        var enemyHealth = new EnemyHealthHandler(_data.Health);
        _damageHandler = enemyHealth;
        _health = enemyHealth;
        enemyHealth.Dead += OnDead;
    }

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
        ObjectPool.Release(_gameObject);
    }

    protected override void OnDead()
    {
        ObjectPool.Get(_data.Explosion, _transform.position,
         _data.Explosion.transform.rotation);

        _events.AddScore?.Invoke(_data.Score);
        _events.EnemyDestroyed?.Invoke(_data.EnemyStrenght);

        _soundPlayer.PlayOneShot(_data.ExplosionSound, _data.ExplosionSoundVolume);

        ObjectPool.Release(_gameObject);
    }
}