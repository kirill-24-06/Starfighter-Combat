using System;
using UnityEngine;

namespace Refactoring
{
    [CreateAssetMenu(fileName = "New PlayerData", menuName = "ScriptableObjects/Player/Data", order = 52)]
    public class PlayerData : ScriptableObject, IMovementData, IDamagebleData, IHealableData, IBonusHandlerData, IWeaponData
    {
        [field: SerializeField] public int Health { get; set; }
        [field: SerializeField] public int MaxHealth { get; set; }

        [SerializeField] private float _tempInvunrabilityTimeSeconds;
        public float TempInvunrabilityTime => _tempInvunrabilityTimeSeconds;

        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public Vector2 GameBorders { get; set; }

        [field: SerializeField] public PlayerLaserSettings ProjectileSettings { get; private set;}
        public GameObject Projectile => ProjectileSettings.Prefab.gameObject;

        [field: SerializeField] public float ReloadCountDown { get; private set; }
        [field: SerializeField] public AudioClip FireSound { get; private set; }
        [field: SerializeField, Range(0.1f, 1)] public float FireSoundVolume { get; private set; }

        [field: SerializeField] public GameObject NukePrefab { get; set; }
        [field: SerializeField] public int NukesStartAmount { get; set; }
        [SerializeField] private float _nukeCd;
        public float NukeCooldown => _nukeCd;
        [field: SerializeField] public float BonusLenght { get; set; }

        [field: SerializeField] public GameObject Explosion { get; set; }
        [field: SerializeField] public AudioClip ExplosionSound { get; set; }
        [field: SerializeField, Range(0.1f, 1)] public float ExplosionSoundVolume { get; set; }

    }

    public interface IBonusHandlerData
    {
        public GameObject NukePrefab { get; set; }

        public int NukesStartAmount { get; set; }

        public float BonusLenght { get; set; }

        public float TempInvunrabilityTime { get; }

        public float NukeCooldown { get; }
    }
}
