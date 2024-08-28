using UnityEngine;

[CreateAssetMenu(fileName = "New RocketShipData", menuName = "Config Data/RocketShip Data", order = 53)]
public class RocketShipData : ScriptableObject
{
    [SerializeField] private ObjectTag _tag;

    [SerializeField] private int _health;

    [SerializeField] private float _speed;

    [SerializeField] private int _score;

    [SerializeField] private Vector2 _disableBorders;

    [SerializeField] private Vector2 _engageZone;

    [SerializeField] private EnemyMissile _enemyMissile;

    [SerializeField] private float _reloadCountDown;

    [SerializeField] private float _liveTime;

    [SerializeField] private int _shootsBeforeRetreat;

    public ObjectTag Tag => _tag;

    public int Health => _health;

    public float Speed => _speed;

    public int Score => _score;

    public Vector2 DisableBorders => _disableBorders;

    public Vector2 EngageZone => _engageZone;

    public EnemyMissile EnemyMissile => _enemyMissile;

    public float ReloadCountDown => _reloadCountDown;

    public float LiveTime => _liveTime;

    public int ShootsBeforeRetreat => _shootsBeforeRetreat;
}
