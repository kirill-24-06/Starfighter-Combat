using UnityEngine;

public class Fighter : Enemy
{
    [SerializeField] private FighterData _data;

    private IAttacker _attacker;
    private IMover _mover;

    private AudioSource _audioPlayer;

    protected override void Awake()
    {
        base.Awake();
        Initialise();
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
        _attacker.Reset();
    }

    protected override void Update()
    {
        base.Update();

        _attacker.Fire(_data.EnemyProjectile.gameObject);
    }

    protected override void Initialise()
    {
        _mover = new Mover(transform);
        _attacker = new EnemyAttacker(this, _data);

        _audioPlayer = EntryPoint.Instance.GlobalSoundFX;

        Data = _data;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    protected override void Move()
    {
        _mover.Move(Vector2.up, _data.Speed);

        DeactivateOutOfBounds(_data.DisableBorders);
    }
    protected override void Disable()
    {
        ObjectPoolManager.SpawnObject(_data.Explosion, transform.position,
            _data.Explosion.transform.rotation, ObjectPoolManager.PoolType.ParticleSystem);

        ObjectPoolManager.ReturnObjectToPool(gameObject);

        _events.AddScore?.Invoke(_data.Score);
        _events.EnemyDestroyed?.Invoke(_data.EnemyStrenght);

        _audioPlayer.PlayOneShot(_data.ExplosionSound, _data.ExplosionSoundVolume);
    }
}