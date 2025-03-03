using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "Missile Settings", menuName = "ScriptableObjects/SpawnSettings/Weapon/Enemy/Missile", order = 2)]
    public class EnemyMissileSettings : MissileSettings<EnemyMissile>
    {
        public override EnemyMissile Create()
        {
            var obj = Instantiate(Prefab);
            var go = obj.gameObject;
            go.SetActive(false);
            PoolRootMap.SetParrentObject(go, GlobalConstants.PoolTypesByTag[Tag]);
            go.name = Prefab.name;

            return obj;
        }

        public override void OnGet(EnemyMissile obj) => obj.OnGet().gameObject.SetActive(true);

        public override void OnRelease(EnemyMissile obj) => obj.OnRelease().gameObject.SetActive(false);
    }
}
