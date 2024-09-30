using UnityEngine;

[CreateAssetMenu(fileName = "New FighterData", menuName = "Config Data/Spawnable Data/Enemy/ Fighter", order = 53)]
public class FighterData : SpawnableData, IData
{
    [Header("Fighter")]
    [SerializeField] private EnemyStrenght _enemyStrenght;

    [SerializeField] private int _health;

    [SerializeField] private EnemyProjectile _enemyProjectile;
    [SerializeField] private float _reloadCountDown;

    [SerializeField] private GameObject _explosionPrefab;
   

    public GameObject Explosion => _explosionPrefab;
    public EnemyStrenght EnemyStrenght => _enemyStrenght;

    public int Health => _health;

    public EnemyProjectile EnemyProjectile => _enemyProjectile;

    public float ReloadCountDown => _reloadCountDown;

}
