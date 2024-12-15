using UnityEngine;


[CreateAssetMenu(fileName = "New PlayerData", menuName = "Config Data/Player Data", order = 52)]
public class PlayerData : ScriptableObject,IMovementData,IDamagebleData, IBonusHandlerData, IShooterData
{
    [field: SerializeField] public int Health { get; set; }
    [field: SerializeField] public int MaxHealth { get; set; }
    [SerializeField] private float _tempInvunrabilityTimeSeconds;
    public int TempInvunrabilityTime => (int)(_tempInvunrabilityTimeSeconds * GlobalConstants.MillisecondsConverter);

    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public Vector2 GameBorders { get; set; }

    [field: SerializeField] public GameObject Projectile { get; private set; }
    [field: SerializeField] public float ReloadCountDown { get; private set; }
    [field: SerializeField] public AudioClip FireSound { get; private set; }
    [field: SerializeField, Range(0.1f, 1)] public float FireSoundVolume { get; private set; }

    [field: SerializeField] public GameObject NukePrefab { get;  set; }
    [field: SerializeField] public int NukesStartAmount { get;  set; }
    [SerializeField] private float _nukeCd;
    public int NukeCooldown => (int)(_nukeCd * GlobalConstants.MillisecondsConverter);
    [field: SerializeField] public float BonusLenght { get;  set; }

    [field: SerializeField] public GameObject ExplosionPrefab { get; set; }
    [field: SerializeField] public AudioClip ExplosionSound { get; set; }
    [field: SerializeField, Range(0.1f, 1)] public float ExplosionSoundVolume { get; set; }
}
