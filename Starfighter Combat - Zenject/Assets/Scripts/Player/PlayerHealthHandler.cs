using UnityEngine;

public class PlayerHealthHandler : IDamageble, IHealable, IResetable
{
    private Player _player;
    private AudioSource _playerAudio;
    private PlayerData _playerData;

    private EventManager _events;
    private int _playerHealth;

    public PlayerHealthHandler(Player player)
    {
        _player = player;
        _playerData = _player.PlayerData;
        _playerAudio = EntryPoint.Instance.GlobalSoundFX;
        _playerHealth = _playerData.Health;

        _events = EntryPoint.Instance.Events;

        _events.PlayerDamaged += TakeDamage;
        _events.PlayerHealed += Heal;
    }

    public void TakeDamage(int damage)
    {
        if (_player.IsInvunerable || damage <= 0)
            return;

        _playerHealth -= damage;

        if (_playerHealth < 0)
        {
            _playerHealth = 0;
        }

        _events.ChangeHealth?.Invoke(_playerHealth);

        if (_playerHealth == 0)
        {
            _events.PlayerDied?.Invoke();
        }

        else
        {
            EntryPoint.Instance.SpawnController.RespawnPlayer();
            EntryPoint.Instance.Events.PlayerRespawn?.Invoke();
            _player.gameObject.SetActive(false);
        }

        ObjectPool.Get(_playerData.Explosion, _player.transform.position,
            _playerData.Explosion.transform.rotation);

        _playerAudio.PlayOneShot(_playerData.ExplosionSound, _playerData.ExplosionSoundVolume);
    }

    public void Heal(int healthAmount)
    {
        _playerHealth += healthAmount;

        if (_playerHealth > _playerData.MaxHealth)
        {
            _playerHealth = _playerData.MaxHealth;
            _events.AddScore?.Invoke(50);
        }

        _events.ChangeHealth?.Invoke(_playerHealth);
    }

    public void Reset()
    {
        _events.PlayerDamaged -= TakeDamage;
        _events.PlayerHealed -= Heal;
    }
}