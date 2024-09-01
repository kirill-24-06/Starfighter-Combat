using UnityEngine;

[CreateAssetMenu(fileName = "New RocketShipData", menuName = "Config Data/Spawnable Data/Enemy/RocketShip", order = 53)]
public class RocketShipData : SpawnableData, IData
{
    [Header("RocketShip")]
    [SerializeField] private EnemyStrenght _enemyStrenght;

    [SerializeField] private int _health;

    [SerializeField] private Vector2 _engageZone;

    [SerializeField] private EnemyMissile _enemyMissile;

    [SerializeField] private float _reloadCountDown;

    [SerializeField] private float _liveTime;

    [SerializeField] private int _shootsBeforeRetreat;
    

    public EnemyStrenght EnemyStrenght => _enemyStrenght;

    public int Health => _health;

    public Vector2 EngageZone => _engageZone;

    public EnemyMissile EnemyMissile => _enemyMissile;

    public float ReloadCountDown => _reloadCountDown;

    public float LiveTime => _liveTime;

    public int ShootsBeforeRetreat => _shootsBeforeRetreat; 
}