using UnityEngine;

[CreateAssetMenu(fileName = "New RocketShipData", menuName = "Config Data/Spawnable Data/Enemy/RocketShip", order = 53)]
public class RocketShipData : SpawnableData, IData, IShooterData,IMovableData
{
    [Header("RocketShip")]
    [SerializeField] private EnemyStrenght _enemyStrenght;

    [SerializeField] private int _health;

    [SerializeField] private Vector2 _engageZone;
    [SerializeField] private float _minY;

    [SerializeField] private EnemyMissile _enemyMissile;

    [SerializeField] private float _reloadCountDown;

    [SerializeField] private AudioClip _fireSound;

    [SerializeField, Range(0.1f, 1)] private float _fireSoundVolume;

    [SerializeField] private float _liveTime;

    [SerializeField] private int _shootsBeforeRetreat;

    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField, Range(0.1f, 1)] private float _explosionSoundVolume;


    public GameObject Explosion => _explosionPrefab;
    public AudioClip ExplosionSound => _explosionSound;
    public float ExplosionSoundVolume => _explosionSoundVolume;

    public EnemyStrenght EnemyStrenght => _enemyStrenght;

    public int Health => _health;

    public Vector2 Area => _engageZone;

    public EnemyMissile EnemyMissile => _enemyMissile;

    public float ReloadCountDown => _reloadCountDown;

    public float LiveTime => _liveTime;

    public int ShootsBeforeRetreat => _shootsBeforeRetreat;

    public AudioClip FireSound => _fireSound;

    public float FireSoundVolume => _fireSoundVolume;

    public float MinY => _minY;
}