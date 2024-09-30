using UnityEngine;

[CreateAssetMenu(fileName = "New StageData", menuName = "Config Data/Level/Stage Data", order = 53)]
public class StageData : ScriptableObject
{
    [SerializeField] private float _stageTime;
    [SerializeField] protected SpawnerData _spawnerData;
  
    public float StageTime => _stageTime;
    public SpawnerData SpawnerData => _spawnerData;
}
