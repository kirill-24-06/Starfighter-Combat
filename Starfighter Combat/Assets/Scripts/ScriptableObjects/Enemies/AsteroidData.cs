using UnityEngine;


[CreateAssetMenu(fileName = "New AsteroidData", menuName = "Config Data/Spawnable Data/Enemy/ Asteroid", order = 53)]
public class AsteroidData : SpawnableData
{
    [Header("Asteroid")]
    [SerializeField] private EnemyStrenght _enemyStrenght;

    [SerializeField] private int _health;
    [SerializeField] private GameObject _explosionPrefb;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField, Range(0.1f, 1)] private float _explosionSoundVolume;

    public GameObject Explosion => _explosionPrefb;

    public EnemyStrenght EnemyStrenght => _enemyStrenght;

    public int Health => _health;

    public AudioClip ExplosionSound => _explosionSound;

    public float ExplosionSoundVolume => _explosionSoundVolume;
}