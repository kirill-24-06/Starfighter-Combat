using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "RocketShipSettings", menuName = "ScriptableObjects/SpawnSettings/Enemy/RocketShip", order = 4)]
    public class RocketShipSettings : AdvancedEnemySpawnSettings<RocketShip>
    {
        public override RocketShip Create()
        {
            var obj = Instantiate(Prefab);
            var go = obj.gameObject;
            go.SetActive(false);
            PoolRootMap.SetParrentObject(go, GlobalConstants.PoolTypesByTag[Tag]);
            go.name = Prefab.name;

            return obj;
        }

        public override void OnGet(RocketShip obj) => obj.OnGet().gameObject.SetActive(true);

        public override void OnRelease(RocketShip obj) => obj.OnRelease().gameObject.SetActive(false);
    }
}
