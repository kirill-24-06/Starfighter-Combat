using System.Collections.Generic;
using UnityEngine;


namespace Refactoring
{
    [CreateAssetMenu(fileName = "New LevelData", menuName = "ScriptableObjects/Level/Level Data", order = 53)]
    public class LevelData : ScriptableObject
    {
        [field:SerializeField] public List<StageData> Stages { get; set; }
        [field: SerializeField] public BossWave BossWave {  get; set; }
        [field: SerializeField] public float BossWaveDelay {  get; set; }

    }
}
