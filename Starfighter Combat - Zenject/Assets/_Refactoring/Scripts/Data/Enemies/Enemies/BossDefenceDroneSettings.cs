using System;
using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "Boss Defence Drone Settings", menuName = "ScriptableObjects/SpawnSettings/Enemy/Boss Defence Drone", order = 5)]
    public class BossDefenceDroneSettings : AdvancedEnemySpawnSettings<BossDefenceDrone>, IPatrolData
    {
        [field: Header("Patrol Data")]
        [field: SerializeField] public float AwaitTime { get; set; }

        [field: Header("Boss Defence Drone")]
        [field: SerializeField,Range(0f,1f)] public float NukeResistModifier { get; private set; }

        public override BossDefenceDrone Create()
        {
            var obj = Instantiate(Prefab);
            var go = obj.gameObject;
            go.SetActive(false);
            PoolRootMap.SetParrentObject(go, GlobalConstants.PoolTypesByTag[Tag]);
            go.name = Prefab.name;

            return obj;
        }

        public override void OnGet(BossDefenceDrone obj) => obj.OnGet().gameObject.SetActive(true);

        public override void OnRelease(BossDefenceDrone obj) => obj.OnRelease().gameObject.SetActive(false);
    }
}

