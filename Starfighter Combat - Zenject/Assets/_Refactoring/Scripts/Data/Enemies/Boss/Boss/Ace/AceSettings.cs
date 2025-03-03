using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "Ace Settings", menuName = "ScriptableObjects/BossSettings/SpawnSettings/Ace", order = 3)]
    public class AceSettings : BossSpawnSettings<EnemyAce>, IAdvancedMovableData
    {
        [field: SerializeField] public Vector3 GameArea { get; set; }
        [field: SerializeField] public float LowerYConstraint { get; set; }

        public override EnemyAce Create()
        {
            var obj = Instantiate(Prefab);
            var go = obj.gameObject;
            go.SetActive(false);
            PoolRootMap.SetParrentObject(go, GlobalConstants.PoolTypesByTag[Tag]);
            go.name = Prefab.name;

            return obj;
        }

        public override void OnGet(EnemyAce obj) => obj.OnGet().gameObject.SetActive(true);

        public override void OnRelease(EnemyAce obj) => obj.OnRelease().gameObject.SetActive(false);
    }
}