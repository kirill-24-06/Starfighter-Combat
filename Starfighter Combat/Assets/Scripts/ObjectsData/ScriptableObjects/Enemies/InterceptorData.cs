using UnityEngine;


[CreateAssetMenu(fileName = "New InterceptorData", menuName = "Config Data/Spawnable Data/Enemy/Interceptor", order = 53)]
public class InterceptorData : SpawnableData, IData
{
    [Header("Interceptor")]
    [SerializeField] private EnemyStrenght _enemyStrenght;

    [SerializeField] private int _health;

    [SerializeField] private Vector2 _engageZone;

    [SerializeField] private EnemyProjectile _enemyProjectile;

    [SerializeField] private float _reloadCountDown;

    [SerializeField] private float _liveTime;

    [SerializeField] private int _shootsBeforeRetreat;

    [SerializeField] private GameObject _explosionPrefab;

    public GameObject Explosion => _explosionPrefab;

    public EnemyStrenght EnemyStrenght => _enemyStrenght;

    public int Health => _health;

    public Vector2 EngageZone => _engageZone;

    public EnemyProjectile EnemyProjectile => _enemyProjectile;

    public float ReloadCountDown => _reloadCountDown;

    public float LiveTime => _liveTime;

    public int ShootsBeforeRetreat => _shootsBeforeRetreat;
   
}
