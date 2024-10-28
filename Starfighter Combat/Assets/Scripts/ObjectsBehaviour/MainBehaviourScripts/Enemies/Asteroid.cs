using UnityEngine;

public class Asteroid : Enemy
{
    [SerializeField] private AsteroidData _data;

    private IMover _mover;
    private AudioSource _soundPlayer;

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
    }

    protected override void Initialise()
    {
        _mover = new Mover(transform);
        _soundPlayer = EntryPoint.Instance.GlobalSoundFX;

        Data = _data;
    }

    protected override void Move()
    {
        _mover.Move(Vector2.up,_data.Speed);

        DeactivateOutOfBounds(_data.DisableBorders);
    }

    protected override void Disable()
    {
        ObjectPoolManager.SpawnObject(_data.Explosion, transform.position,
            _data.Explosion.transform.rotation, ObjectPoolManager.PoolType.ParticleSystem);

        ObjectPoolManager.ReturnObjectToPool(gameObject);

        _events.AddScore?.Invoke(_data.Score);
        _events.EnemyDestroyed?.Invoke(_data.EnemyStrenght);

        _soundPlayer.PlayOneShot(_data.ExplosionSound, _data.ExplosionSoundVolume);
    }
}