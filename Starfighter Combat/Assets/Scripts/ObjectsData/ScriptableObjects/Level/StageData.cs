using UnityEngine;

[CreateAssetMenu(fileName = "New StageData", menuName = "Config Data/Level/Stage Data", order = 53)]
public class StageData : ScriptableObject
{
    //[SerializeField] private LevelStage _stage;
    [SerializeField] private float _stageTime;
    [SerializeField] private SpawnerData _spawnerData;

    //public LevelStage Stage => _stage;
    public float StageTime => _stageTime;
    public SpawnerData SpawnerData => _spawnerData;
}
