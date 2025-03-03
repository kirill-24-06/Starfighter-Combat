using UnityEngine;


namespace Refactoring
{
    [CreateAssetMenu(fileName = "New StageData", menuName = "ScriptableObjects/Level/Stage Data", order = 53)]
    public class StageData : ScriptableObject
    {
        [SerializeField] private float _stageTime;
        [SerializeField] protected SpawnerData _spawnerData;

        public float StageTime => _stageTime;
        public SpawnerData SpawnerData => _spawnerData;
    }
}
