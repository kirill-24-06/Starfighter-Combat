using System.Collections.Generic;
using UnityEngine;
using Utils.Serializer;


namespace Refactoring
{
    [CreateAssetMenu(fileName = "New SpawnerData", menuName = "ScriptableObjects/Level/Spawner Data", order = 2)]
    public class SpawnerData : ScriptableObject
    {
        [field: Header("Unit Spawn Settings")]

        [field: SerializeField, Tooltip("Must implement IUnitSpawnSettings interface")]
        public List<InterfaceReference<IEnemySpawnSettings>> Enemies { get; private set; }

        [field: SerializeField, Tooltip("Must implement IUnitSpawnSettings interface")]
        public List<InterfaceReference<IEnemySpawnSettings>> EliteEnemies { get; private set; }

        [field: SerializeField, Tooltip("Must implement IUnitSpawnSettings interface")]
        public List<InterfaceReference<IEnemySpawnSettings>> Bonuses { get; private set; }

        [field: Header("Spawn Timings")]
        [field: SerializeField] public float SpawnDelay { get; private set; }
        [field: SerializeField] public float SpawnTime { get; private set; }
        [field: SerializeField] public float EliteSpawnTime { get; private set; }
        [field: SerializeField] public float BonusSpawnTime { get; private set; }

        [field: Header("Amount of Enemies")]
        [field: SerializeField] public int MaxEnemies { get; private set; }
        [field: SerializeField] public int MaxHardEnemies { get; private set; }
    }
}