using UnityEngine;

public class EnemyAce : Boss
{
    private BossShieldBehaviour _bossShield;

    protected override void Awake()
    {
        base.Awake();

        PoolMap.SetParrentObject(_gameObject, GlobalConstants.PoolTypesByTag[_data.Tag]);
    }

    private void Start()
    {
        Initialise();

        var collider = GetComponent<PolygonCollider2D>();
        EntryPoint.Instance.CollisionMap.Register(collider, this);
        EntryPoint.Instance.CollisionMap.RegisterNukeInteractable(collider, this);
        EntryPoint.Instance.CollisionMap.RegisterMissileTarget(transform);
        EntryPoint.Instance.Events.BossArrival?.Invoke();
    }

    private void OnEnable()
    {
        _health?.Reset();
        _isInPool = false;
    }

    protected override void Initialise()
    {
        base.Initialise();

        var health = new BossHealthHandler(_data.MaxHealth, _events);
        _damageHandler = health;
        _health = health;

        health.Dead += OnDeath;
        health.HealthChanged = (int newHealth) => _currentHealth = newHealth;

        _bossShield = transform.Find("BossShield").gameObject.GetComponent<BossShieldBehaviour>();
    }

    protected override void TakeDamage(int damage)
    {
        if (damage <= 0 || _bossShield.IsActive || _isInvunerable) return;

        _damageHandler.TakeDamage(damage);
    }

    protected override void OnDeath()
    {
        if (_isInPool) return;
        _isInPool = true;

        ObjectPool.Get(_data.Explosion, _transform.position, _data.Explosion.transform.rotation);

        _events.BossDefeated?.Invoke();
        _audioPlayer.PlayOneShot(_data.ExplosionSound, _data.ExplosionSoundVolume);

        ObjectPool.Release(_gameObject);
    }
}