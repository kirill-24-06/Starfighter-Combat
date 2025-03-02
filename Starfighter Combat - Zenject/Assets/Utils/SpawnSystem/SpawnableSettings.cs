using UnityEngine;

namespace Utils.SpawnSystem
{
    public abstract class SpawnableSettings<T> : ScriptableObject, ISettings<T> where T : MonoProduct
    {
        [field: SerializeField] public T Prefab { get; protected set; }
        [field: SerializeField] public int PrewarmAmount { get; protected set; }

        public abstract T Create();
        public abstract void OnGet(T obj);
        public abstract void OnRelease(T obj);
    }
}
