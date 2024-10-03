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

    protected override void OnEnable()
    {
        base.OnEnable();

        _health = _data.Health;
        _attacker.Reset();
    }

    protected override void Update()
    {
        base.Update();

        _attacker.Fire(_data.EnemyProjectile.gameObject);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

    protected override void Initialise()
    {
        _mover = new Mover(transform);
        _attacker = new EnemyAttacker(this, _data);

        _audioPlayer = EntryPoint.Instance.GlobalSoundFX;

        Data = _data;
    }

    protected override void Move()
    {
        _mover.Move(Vector2.up, _data.Speed);

        DeactivateOutOfBounds(_data.DisableBorders);
    }
    protected override void Disable()
    {
        Instantiate(_data.Explosion,transform.position, _data.Explosion.transform.rotation);

        ObjectPoolManager.ReturnObjectToPool(gameObject);

        _events.AddScore?.Invoke(_data.Score);
        _events.EnemyDestroyed?.Invoke(_data.EnemyStrenght);

        _audioPlayer.PlayOneShot(_data.ExplosionSound, _data.ExplosionSoundVolume);
    }
}

public interface IShooterData
{
    public float ReloadCountDown { get; }

    public AudioClip FireSound { get; }

    public float FireSoundVolume { get; }
}