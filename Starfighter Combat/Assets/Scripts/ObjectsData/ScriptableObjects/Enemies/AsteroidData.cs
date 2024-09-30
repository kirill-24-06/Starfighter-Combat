using UnityEngine;


[CreateAssetMenu(fileName = "New AsteroidData", menuName = "Config Data/Spawnable Data/Enemy/ Asteroid", order = 53)]
public class AsteroidData : SpawnableData,IData
{
    [Header("Asteroid")]
    [SerializeField] private EnemyStrenght _enemyStrenght;

    [SerializeField] private int _health;
    [SerializeField] private GameObject _explosionPrefb;

    public GameObject Explosion => _explosionPrefb;

    public EnemyStrenght EnemyStrenght => _enemyStrenght;

    public int Health => _health;
}