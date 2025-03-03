using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "Missile Settings", menuName = "ScriptableObjects/SpawnSettings/Weapon/Player/ Missile", order = 2)]
    public class PlayerMissileSettings : MissileSettings<PlayerMissile>
    {
        public override PlayerMissile Create()
        {
            var obj = Instantiate(Prefab);
            var go = obj.gameObject;
            go.SetActive(false);
            PoolRootMap.SetParrentObject(go, GlobalConstants.PoolTypesByTag[Tag]);
            go.name = Prefab.name;

            return obj;
        }

        public override void OnGet(PlayerMissile obj) => obj.OnGet().gameObject.SetActive(true);

        public override void OnRelease(PlayerMissile obj) => obj.OnRelease().gameObject.SetActive(false);
    }
}