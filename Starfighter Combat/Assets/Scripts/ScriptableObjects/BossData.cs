using UnityEngine;

public abstract class BossData : SpawnableData,IMovableData
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private BossStageData[] _stages;
    [SerializeField] private Vector2 _gameArea;
    [SerializeField] private float _minY;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField, Range(0.1f, 1)] private float _explosionSoundVolume;
    [SerializeField] private AudioClip _bossMusic;

    public GameObject Explosion => _explosion;
    public AudioClip ExplosionSound => _explosionSound;
    public float ExplosionSoundVolume => _explosionSoundVolume;

    public int MaxHealth => _maxHealth;
    public BossStageData[] Stages => _stages;
    public Vector2 Area => _gameArea;
    public float MinY => _minY;

    public AudioClip BossMusic => _bossMusic;
}