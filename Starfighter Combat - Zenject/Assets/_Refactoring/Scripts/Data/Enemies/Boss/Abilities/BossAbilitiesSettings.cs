using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "Boss Abilities Settings", menuName = "ScriptableObjects/BossSettings/BossAbilities")]
    public class BossAbilitiesSettings : ScriptableObject
    {
        [field: SerializeField] public MinionSettings[] Minions { get; private set; }
    }
}