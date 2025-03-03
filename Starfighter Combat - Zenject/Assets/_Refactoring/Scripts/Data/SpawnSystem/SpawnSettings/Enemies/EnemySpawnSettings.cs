using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public abstract class EnemySpawnSettings<T> : UnitSpawnSettings<T>, IMovableData, IDamagebleData where T : MonoProduct
    {
        [field: Header("MovableData")]
        [field: SerializeField] public Vector3 Direction { get ; set ; }
        [field: SerializeField] public float Speed { get ; set ; }

        [field: Header("Damageble Data")]
        [field: SerializeField] public GameObject Explosion { get; set; }
        [field: SerializeField] public int Health { get ; set ; }
        [field: SerializeField] public AudioClip ExplosionSound { get ; set ; }
        [field: SerializeField,Range(0.1f,1f)] public float ExplosionSoundVolume { get ; set; }

        [field: Header("Enemy Data")]
        [field: SerializeField] public EnemyStrenght EnemyStrenght { get; set; }
    }
}

