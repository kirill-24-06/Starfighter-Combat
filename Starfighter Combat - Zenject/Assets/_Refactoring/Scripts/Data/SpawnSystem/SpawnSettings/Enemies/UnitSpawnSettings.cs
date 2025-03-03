using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public abstract class UnitSpawnSettings<T> : GlobalSpawnSettings<T>, IEnemySpawnSettings where T : MonoProduct
    {
        [field: SerializeField] public int Score { set; get; }
        [field: SerializeField] public AreaTag[] SpawnZones { get; set; }
        [field: SerializeField] public bool UsePrefabRotation { get; set; } = false;

        public MonoProduct UnitToSpawn => Prefab;

    }

    public interface IEnemySpawnSettings
    {
        public MonoProduct UnitToSpawn { get; }
        public AreaTag[] SpawnZones { get; set; }
        public bool UsePrefabRotation { get; set; }
    }
}

