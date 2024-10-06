using UnityEngine;

public abstract class BossData : SpawnableData
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private BossStageData[] _stages;
    [SerializeField] private Vector2 _gameArea;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField, Range(0.1f, 1)] private float _explosionSoundVolume;

    public GameObject Explosion => _explosion;
    public AudioClip ExplosionSound => _explosionSound;
    public float ExplosionSoundVolume => _explosionSoundVolume;

    public int MaxHealth => _maxHealth;
    public BossStageData[] Stages => _stages;
    public Vector2 GameArea => _gameArea;
}