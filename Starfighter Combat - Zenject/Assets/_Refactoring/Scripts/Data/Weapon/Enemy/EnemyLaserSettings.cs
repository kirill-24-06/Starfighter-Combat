using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "Laser Settings", menuName = "ScriptableObjects/SpawnSettings/Weapon/Enemy/Laser", order = 1)]
    public class EnemyLaserSettings : ProjectileSettings<EnemyLaser>
    {
        public override EnemyLaser Create()
        {
            var obj = Instantiate(Prefab);
            var go = obj.gameObject;
            go.SetActive(false);
            PoolRootMap.SetParrentObject(go, GlobalConstants.PoolTypesByTag[Tag]);
            go.name = Prefab.name;

            return obj;
        }

        public override void OnGet(EnemyLaser obj) => obj.OnGet().gameObject.SetActive(true);

        public override void OnRelease(EnemyLaser obj) => obj.OnRelease().gameObject.SetActive(false);
    }
}
