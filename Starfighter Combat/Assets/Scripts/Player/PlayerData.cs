using UnityEngine;


[CreateAssetMenu(fileName = "New PlayerData", menuName = "Config Data/Player Data", order = 52)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private ObjectTag _tag;
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _tempInvunrabilityTimeSeconds;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _nukePrefab;
    [SerializeField] private float _nukeCd;
    [SerializeField] private int _nukesStartAmount;
    [SerializeField] private GameObject _weapon;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _bonusLenght;
    [SerializeField] private Vector2 _gameBorders;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AudioClip _fireSound;
    [SerializeField, Range(0.1f, 1)] private float _fireSoundVolume;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField, Range(0.1f, 1)] private float _explosionSoundVolume;

    public ObjectTag Tag => _tag;

    public int Health => _health;

    public int MaxHealth => _maxHealth;

    public int TempInvunrabilityTimeMilliseconds => (int)(_tempInvunrabilityTimeSeconds * GlobalConstants.MillisecondsConverter);

    public float Speed => _speed;

    public GameObject Projectile => _weapon;

    public float ReloadTime => _reloadTime;

    public Vector2 GameZoneBorders => _gameBorders;

    public float BonusTimeLenght => _bonusLenght;

    public GameObject NukePrefab => _nukePrefab;
    public int NukesStartAmount => _nukesStartAmount;

    public GameObject Explosion => _explosionPrefab;

    public AudioClip FireSound => _fireSound;

    public float FireSoundVolume => _fireSoundVolume;

    public AudioClip ExplosionSound => _explosionSound;

    public float ExplosionSoundVolume => _explosionSoundVolume;

    public int NukeCooldown => (int)(_nukeCd * GlobalConstants.MillisecondsConverter);
}
