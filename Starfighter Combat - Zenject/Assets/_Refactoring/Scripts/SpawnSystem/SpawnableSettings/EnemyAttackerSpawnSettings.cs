using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public abstract class EnemyAttackerSpawnSettings<T> : EnemySpawnSettings<T>, IWeaponData where T : MonoProduct
    {
        [field: Header("Weapon Data")]
        [field: SerializeField]public float ReloadCountDown { get; set; }
        [field: SerializeField] public AudioClip FireSound { get; set; }
        [field: SerializeField,Range(0.1f,1f)] public float FireSoundVolume { get; set; }
        [field: SerializeField] public GameObject Projectile { get; set; }
    }

    public interface IAdvancedMovableData: IMovableData
    {
        public Vector3 GameArea { get;}

        public float LowerYConstraint {  get;  }
    }

    public interface IAdvancedWeaponData : IWeaponData
    {
        public int AttacksBeforeReposition { get; }
    }
}

