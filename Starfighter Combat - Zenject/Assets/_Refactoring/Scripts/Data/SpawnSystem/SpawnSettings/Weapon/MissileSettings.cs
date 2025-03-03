using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public abstract class MissileSettings<T> : ProjectileSettings<T>, IMissileBaseData where T : MonoProduct
    {
        [field: Header("Missile Data")]
        [field: SerializeField] public float LaunchTime { get; set; }
        [field: SerializeField] public float HomingTime { get; set; }
    }
}

