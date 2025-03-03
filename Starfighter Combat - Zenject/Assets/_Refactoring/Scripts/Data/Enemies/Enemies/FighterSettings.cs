using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "FighterSettings", menuName = "ScriptableObjects/SpawnSettings/Enemy/Fighter", order = 2)]
    public class FighterSettings : EnemyAttackerSpawnSettings<Fighter>
    {
        public override Fighter Create()
        {
            var obj = Instantiate(Prefab);
            var go = obj.gameObject;
            go.SetActive(false);
            PoolRootMap.SetParrentObject(go, GlobalConstants.PoolTypesByTag[Tag]);
            go.name = Prefab.name;

            return obj;
        }

        public override void OnGet(Fighter obj) => obj.OnGet().gameObject.SetActive(true);

        public override void OnRelease(Fighter obj) => obj.OnRelease().gameObject.SetActive(false);
    }
}

