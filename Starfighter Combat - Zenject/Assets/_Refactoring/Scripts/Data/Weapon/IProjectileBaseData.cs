using UnityEngine;

namespace Refactoring
{
    public interface IProjectileBaseData
    {
        public GameObject ExplosionPrefab { get; set; }

        public GameObject CollideEffect { get; set; }

        public ObjectTag Tag { get; set; }
    }
}
