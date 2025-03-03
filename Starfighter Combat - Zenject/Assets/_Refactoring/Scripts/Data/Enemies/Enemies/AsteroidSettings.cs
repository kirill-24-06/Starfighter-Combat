using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "AsteroidSettings", menuName = "ScriptableObjects/SpawnSettings/Enemy/ Asteroid", order = 1)]
    public class AsteroidSettings : EnemySpawnSettings<Asteroid>
    {
        public override Asteroid Create()
        {
            var obj = Instantiate(Prefab);
            var go = obj.gameObject;
            go.SetActive(false);
            PoolRootMap.SetParrentObject(go, GlobalConstants.PoolTypesByTag[Tag]);
            go.name = Prefab.name;

            return obj;
        }

        public override void OnGet(Asteroid obj) => obj.OnGet().gameObject.SetActive(true);

        public override void OnRelease(Asteroid obj) => obj.OnRelease().gameObject.SetActive(false);
    }
}

