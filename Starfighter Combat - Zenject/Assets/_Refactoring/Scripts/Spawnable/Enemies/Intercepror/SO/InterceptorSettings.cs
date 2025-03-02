using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "InterceptorSettings", menuName = "ScriptableObjects/SpawnSettings/Enemy/Interceptor", order = 3)]
    public class InterceptorSettings : AdvancedEnemySpawnSettings<Interceptor>
    {
        

        public override Interceptor Create()
        {
            var obj = Instantiate(Prefab);
            var go = obj.gameObject;
            go.SetActive(false);
            PoolRootMap.SetParrentObject(go, GlobalConstants.PoolTypesByTag[Tag]);
            go.name = Prefab.name;

            return obj;
        }

        public override void OnGet(Interceptor obj) => obj.OnGet().gameObject.SetActive(true);

        public override void OnRelease(Interceptor obj) => obj.OnRelease().gameObject.SetActive(false);
    }

    public interface ILifetimeSettings
    {
        public float LifeTime { get; set; }
    }
}

