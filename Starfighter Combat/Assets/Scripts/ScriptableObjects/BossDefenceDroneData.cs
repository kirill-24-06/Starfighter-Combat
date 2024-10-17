using UnityEngine;

[CreateAssetMenu(fileName = "New BossDefenceDrone", menuName = "Config Data/Spawnable Data/Enemy/BossDefenceDrone", order = 53)]
public class BossDefenceDroneData:SpawnableData,IShooterData, IMovableData
{
    [SerializeField] private int _health;
    [SerializeField] private Vector2 _engageZone;
    [SerializeField] private float _minY;

    [SerializeField] private EnemyMissile _enemyMissile;
    [SerializeField] private float _reloadCountDown;

    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField, Range(0.1f, 1)] protected float _explosionSoundVolume;

    [SerializeField] private AudioClip _fireSound;
    [SerializeField, Range(0.1f, 1)] private float _fireSoundVolume;


    public GameObject Explosion => _explosionPrefab;
    public AudioClip ExplosionSound => _explosionSound;
    public float ExplosionSoundVolume => _explosionSoundVolume;

    public Vector2 Area => _engageZone;

    public int Health => _health;

    public EnemyMissile EnemyMissile => _enemyMissile;

    public float ReloadCountDown => _reloadCountDown;

    public AudioClip FireSound => _fireSound;

    public float FireSoundVolume => _fireSoundVolume;

    public float MinY => _minY;
}