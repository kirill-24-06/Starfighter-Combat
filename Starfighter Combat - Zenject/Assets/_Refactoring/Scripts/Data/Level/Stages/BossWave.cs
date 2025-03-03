using UnityEngine;
using Utils.Serializer;
using Utils.SpawnSystem;


namespace Refactoring
{
    [CreateAssetMenu(fileName = "New BossWave", menuName = "ScriptableObjects/Level/Boss Wave", order = 53)]
    public class BossWave : StageData
    {
        [field: SerializeField] public InterfaceReference<IEnemySpawnSettings>[] Bosses {  get; set; }
    }
}
