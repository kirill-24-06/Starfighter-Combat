using UnityEngine;



[CreateAssetMenu(fileName = "New SpawnableData", menuName = "Config Data/Spawnable Data", order = 53)]
public abstract class SpawnableData : ScriptableObject
{
    [Header("Base")]
    [SerializeField] protected GameObject _prefab;

    [SerializeField] protected ObjectTag _tag;

    [SerializeField] protected float _speed;

    [SerializeField] protected int _score;

    [SerializeField] protected Vector2 _disableBorders;

    [SerializeField] protected AreaTag[] _spawnZones;

    public GameObject Prefab => _prefab;
    
    public ObjectTag Tag => _tag;

    public float Speed => _speed;

    public int Score => _score;

    public Vector2 DisableBorders => _disableBorders;


    public AreaTag[] SpawnZones => _spawnZones;
}
