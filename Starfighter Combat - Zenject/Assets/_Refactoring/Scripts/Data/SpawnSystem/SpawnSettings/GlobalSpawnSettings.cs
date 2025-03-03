using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public abstract class GlobalSpawnSettings<T> : SpawnableSettings<T> where T : MonoProduct
    {
        [field: Header("Global")]
        [field: SerializeField] public ObjectTag Tag { get; set; }
    }
}

