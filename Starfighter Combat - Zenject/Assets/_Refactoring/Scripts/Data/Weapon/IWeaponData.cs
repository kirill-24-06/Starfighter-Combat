using UnityEngine;

namespace Refactoring
{
    public interface IWeaponData
    {
        public float ReloadCountDown { get; }

        public AudioClip FireSound { get; }

        public float FireSoundVolume { get; }

        public GameObject Projectile { get; }
    }
}
