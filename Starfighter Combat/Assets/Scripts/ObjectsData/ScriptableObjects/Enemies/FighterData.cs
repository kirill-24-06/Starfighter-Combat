using UnityEngine;

[CreateAssetMenu(fileName = "New FighterData", menuName = "Config Data/Spawnable Data/Enemy/ Fighter", order = 53)]
public class FighterData : SpawnableData, IData, IShooterData
{
    [Header("Fighter")]
    [SerializeField] private EnemyStrenght _enemyStrenght;

    [SerializeField] private int _health;

    [SerializeField] private EnemyProjectile _enemyProjectile;
    [SerializeField] private float _reloadCountDown;

    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField, Range(0.1f, 1)] protected float _explosionSoundVolume;

    [SerializeField] private AudioClip _fireSound;
    [SerializeField, Range(0.1f, 1)] private float _fireSoundVolume;


    public GameObject Explosion => _explosionPrefab;
    public AudioClip ExplosionSound => _explosionSound;
    public float ExplosionSoundVolume => _explosionSoundVolume;

    public EnemyStrenght EnemyStrenght => _enemyStrenght;

    public int Health => _health;

    public EnemyProjectile EnemyProjectile => _enemyProjectile;

    public float ReloadCountDown => _reloadCountDown;

    public AudioClip FireSound => _fireSound;

    public float FireSoundVolume => _fireSoundVolume;
}
