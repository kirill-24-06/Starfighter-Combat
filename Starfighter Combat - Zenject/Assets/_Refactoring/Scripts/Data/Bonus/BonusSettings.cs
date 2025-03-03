using System.Xml.Linq;
using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "Bonus Settings", menuName = "ScriptableObjects/SpawnSettings/Bonus", order = 1)]
    public class BonusSettings : UnitSpawnSettings<Bonus>, IMovableData, ILifetimeSettings, IAdvancedMovableData, IPatrolData
    {
        [field: Header("Bonus Data")]
        [field: SerializeField] public BonusTag BonusTag { get; private set; }
        [field: SerializeField] public float LifeTime { get; set; }

        [field: Header("Movement Data")]
        [field: SerializeField] public Vector3 Direction { get; set; }
        [field: SerializeField] public float Speed { get; set; }
        [field: SerializeField] public Vector3 GameArea { get; set; }
        [field: SerializeField] public float LowerYConstraint { get; set; }

        [field: Header("Patrol Data")]
        [field: SerializeField] public float AwaitTime { get; set; }

        public override Bonus Create()
        {
            var obj = Instantiate(Prefab);
            var go = obj.gameObject;
            go.SetActive(false);
            PoolRootMap.SetParrentObject(go, GlobalConstants.PoolTypesByTag[Tag]);
            go.name = Prefab.name;

            return obj;
        }

        public override void OnGet(Bonus obj) => obj.OnGet().gameObject.SetActive(true);

        public override void OnRelease(Bonus obj) => obj.OnRelease().gameObject.SetActive(false);
    }
}