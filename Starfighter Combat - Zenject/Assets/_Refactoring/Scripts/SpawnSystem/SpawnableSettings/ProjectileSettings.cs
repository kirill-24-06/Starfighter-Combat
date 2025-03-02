using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public abstract class ProjectileSettings<T> : GlobalSpawnSettings<T>, IProjectileSettings, IMovableData, IProjectileBaseData where T : MonoProduct
    {
        [field: Header("Move Data")]
        [field: SerializeField] public Vector3 Direction { get; set; }
        [field: SerializeField] public float Speed { get; set; }

        [field: Header("Projectile Data")]
        [field: SerializeField] public GameObject ExplosionPrefab { get; set; }
        [field: SerializeField] public GameObject CollideEffect { get; set; }
        public MonoProduct PrefabGO => Prefab;
    }

    public interface IProjectileSettings
    {
        public MonoProduct PrefabGO { get; }
    }
    
}

