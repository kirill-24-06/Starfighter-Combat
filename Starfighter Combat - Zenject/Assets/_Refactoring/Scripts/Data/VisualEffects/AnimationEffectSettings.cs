using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "AnimationSpriteSettings", menuName = "ScriptableObjects/SpawnSettings/Effects/AnimationSprite", order = 1)]
    public class AnimationEffectSettings : GlobalSpawnSettings<AnimationEffect>
    {
        [field: SerializeField] public float DestroyTime { get; private set; }

        public override AnimationEffect Create()
        {
            var obj = Instantiate(Prefab);
            var go = obj.gameObject;
            go.SetActive(false);
            PoolRootMap.SetParrentObject(go, GlobalConstants.PoolTypesByTag[Tag]);
            go.name = Prefab.name;

            return obj;
        }

        public override void OnGet(AnimationEffect obj) => obj.OnGet().gameObject.SetActive(true);

        public override void OnRelease(AnimationEffect obj) => obj.OnRelease().gameObject.SetActive(false);
    }
}
