using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public abstract class AdvancedEnemySpawnSettings<T>:EnemyAttackerSpawnSettings<T>,IAdvancedWeaponData,IAdvancedMovableData,ILifetimeSettings where T: MonoProduct
    {
        [field: Header("Advanced Weapon Settings")]
        [field: SerializeField] public int AttacksBeforeReposition { get; set; }

        [field:Header("Advanced Movement Data")]
        [field: SerializeField] public Vector3 GameArea { get ; set; }
        [field: SerializeField] public float LowerYConstraint { get ; set; }

        [field: Header("Other")]
        [field: SerializeField] public float LifeTime { get; set; }
    }
}

