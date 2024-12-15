using UnityEngine;
using Zenject;

public class PlayerHealthHandler : IInitializable, IDamageble, IHealable
{
    private AudioSource _globalSoundFX;
    private EventManager _events;

    private SpawnController _spawner;

    private GameObject _playerGO;
    private Transform _playerTransform;

    private int _maxHealth;
    private int _currentHealth;

    private GameObject _explosion;
    private AudioClip _explosionSound;
    private float _soundVolume;

    public PlayerHealthHandler(GameObject player, IDamagebleData data,  EventManager events, AudioSource globalSound)
    {
        _globalSoundFX = globalSound;
        _playerGO = player;
        _playerTransform = _playerGO.transform;
        _events = events;

        _currentHealth = data.Health;
        _maxHealth = data.MaxHealth;
        _explosion = data.ExplosionPrefab;
        _explosionSound = data.ExplosionSound;
        _soundVolume = data.ExplosionSoundVolume;
    }

    public void Initialize() => _events.ChangeHealth?.Invoke(_currentHealth);

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;

        _currentHealth -= damage;

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        _events.ChangeHealth?.Invoke(_currentHealth);

        if (_currentHealth == 0)
        {
            _events.PlayerDied?.Invoke();
        }

        else
        {
            EntryPoint.Instance.SpawnController.RespawnPlayer();
            _events.PlayerRespawn?.Invoke();
            _playerGO.SetActive(false);
        }

        ObjectPool.Get(_explosion, _playerTransform.position, _explosion.transform.rotation);

        _globalSoundFX.PlayOneShot(_explosionSound, _soundVolume);
    }

    public void Heal(int healthAmount)
    {
        _currentHealth += healthAmount;

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
            _events.AddScore?.Invoke(50);
        }

        _events.ChangeHealth?.Invoke(_currentHealth);
    }

}
