using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public abstract class BossSpawnSettings<T> : UnitSpawnSettings<T>, IDamagebleData, IMovableData, IBossData where T : MonoProduct
    {
        [field: Header("Boss Data")]
        [field: SerializeField] public BossStageSettings[] Stages { get; private set; }
        [field: SerializeField, Range(0f,1.0f)] public float NukeResistModifier { get; private set; }

        [field: Header("Damageble Data")]
        [field: SerializeField] public GameObject Explosion { get;private set; }
        [field: SerializeField] public int Health { get; private set; }
        [field: SerializeField] public AudioClip ExplosionSound { get; private set; }
        [field: SerializeField, Range(0.1f, 1.0f)] public float ExplosionSoundVolume { get; private set; }

        [field: Header("Movable Data")]
        [field: SerializeField] public Vector3 Direction { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

    }

    public interface IBossData
    {
        public BossStageSettings[] Stages { get; }

        public float NukeResistModifier {  get; }
    }
}

