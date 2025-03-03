using System;
using UnityEngine;
using Utils.Serializer;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "Transition Stage Settings", menuName = "ScriptableObjects/BossSettings/BossStages/TransitionStage")]
    public class TransitionStageSettings : BossStageSettings
    {
        [field: SerializeField] public MinionSettings[] Minions {  get; private set; }

        public override Type GetStageBuilder()
        {
            return typeof(TransitionStageBuilder);
        }

    }

        [Serializable]
        public class MinionSettings
        {
            [field: SerializeField] public InterfaceReference<IEnemySpawnSettings> Minion { get; private set; }
            [field: SerializeField] public int AmountToSpawn { get; private set; }
        }
}