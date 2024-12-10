using UnityEngine;



[CreateAssetMenu(fileName = "New SpawnableData", menuName = "Config Data/Spawnable Data", order = 53)]
public abstract class SpawnableData : PrewarmableData
{
    [Header("Base")]
    //[SerializeField] protected GameObject _prefab;
    //[SerializeField] protected int _prewarmAmount;

    //[SerializeField] protected ObjectTag _tag;

    [SerializeField] protected float _speed;

    [SerializeField] protected int _score;

    [SerializeField] protected Vector2 _disableBorders;

    [SerializeField] protected AreaTag[] _spawnZones;

    //public GameObject Prefab => _prefab;
    //public int PrewarmAmount => _prewarmAmount;
    
    //public ObjectTag Tag => _tag;

    public float Speed => _speed;

    public int Score => _score;

    public Vector2 DisableBorders => _disableBorders;


    public AreaTag[] SpawnZones => _spawnZones;
}
