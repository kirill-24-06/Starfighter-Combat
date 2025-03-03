using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "Laser Settings", menuName = "ScriptableObjects/SpawnSettings/Weapon/Player/Laser", order = 1)]
    public class PlayerLaserSettings : ProjectileSettings<PlayerLaser>
    {
        public override PlayerLaser Create()
        {
            var obj = Instantiate(Prefab);
            var go = obj.gameObject;
            go.SetActive(false);
            PoolRootMap.SetParrentObject(go, GlobalConstants.PoolTypesByTag[Tag]);
            go.name = Prefab.name;

            return obj;
        }

        public override void OnGet(PlayerLaser obj) => obj.OnGet().gameObject.SetActive(true);

        public override void OnRelease(PlayerLaser obj) => obj.OnRelease().gameObject.SetActive(false);
    }
}
