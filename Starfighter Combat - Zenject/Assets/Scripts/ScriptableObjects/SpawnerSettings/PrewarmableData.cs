using UnityEngine;

public abstract class PrewarmableData : ScriptableObject
{
    [SerializeField] protected GameObject _prefab;
    [SerializeField] protected int _prewarmAmount;
    [SerializeField] protected ObjectTag _tag;

    public ObjectTag Tag => _tag;
    public GameObject Prefab => _prefab;
    public int PrewarmAmount => _prewarmAmount;
}
