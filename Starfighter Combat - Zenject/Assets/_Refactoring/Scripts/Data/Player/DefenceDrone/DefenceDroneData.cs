using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "Drone Data", menuName = "ScriptableObjects/Player/DefenceDrone/Data", order = 53)]
    public class DefenceDroneData : ScriptableObject, IWeaponData
    {
        [SerializeField] PlayerMissileSettings _missile;
        [SerializeField] private float _reloadCountDown;
        [SerializeField] private AudioClip _fireSound;
        [SerializeField, Range(0.1f, 1)] float _fireSoundVolume;

        public float ReloadCountDown => _reloadCountDown;

        public AudioClip FireSound => _fireSound;

        public float FireSoundVolume => _fireSoundVolume;

        public GameObject Projectile => _missile.Prefab.gameObject;
    }

}