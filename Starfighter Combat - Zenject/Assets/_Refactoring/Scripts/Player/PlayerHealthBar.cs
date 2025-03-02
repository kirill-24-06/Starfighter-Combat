using UnityEngine;
using Utils.Events.Channel.Static;
using Utils.SpawnSystem;
using Zenject;

namespace Refactoring
{
    public class PlayerHealthBar : IInitializable, IDamageble, IHealable
    {
        private IDamagebleData _data;

        private Utils.SpawnSystem.IFactory<MonoProduct> _explosionFactory;

        private GameObject _playerGO;
        private Transform _playerTransform;

        private PlayerDiedEvent _playerDied;
        private PlayerRespawnEvent _playerRespawn;
        private PlayGlobalSoundEvent _playGlobalSound;

        private int _maxHealth;
        private int _currentHealth;


        public PlayerHealthBar(
            GameObject player,
            Utils.SpawnSystem.IFactory<MonoProduct> explosionEffects,
            IDamagebleData data,
            IHealableData healableData)
        {
            _data = data;

            _explosionFactory = explosionEffects;

            _playerGO = player;
            _playerTransform = _playerGO.transform;

            _maxHealth = healableData.MaxHealth;
            _currentHealth = _data.Health;

            _playGlobalSound = new PlayGlobalSoundEvent(_data.ExplosionSound, _data.ExplosionSoundVolume);

        }

        public void Initialize()
        {
            Channel<ChangeHealthEvent>.Raise(new ChangeHealthEvent(_currentHealth));
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0) return;

            _currentHealth -= damage;

            if (_currentHealth < 0)
            {
                _currentHealth = 0;
            }

            Channel<ChangeHealthEvent>.Raise(new ChangeHealthEvent(_currentHealth));

            if (_currentHealth == 0)
            {
                Channel<PlayerDiedEvent>.Raise(_playerDied);
            }

            else
            {
                Channel<PlayerRespawnEvent>.Raise(_playerRespawn);
                _playerGO.SetActive(false);
            }

            var explosion = _explosionFactory.Create();
            explosion.transform.SetLocalPositionAndRotation(_playerTransform.position, explosion.transform.rotation);

            Channel<PlayGlobalSoundEvent>.Raise(_playGlobalSound);
        }

        public void Heal(int healthAmount)
        {
            _currentHealth += healthAmount;

            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
                Channel<AddScoreEvent>.Raise(new AddScoreEvent(50));
            }

            Channel<ChangeHealthEvent>.Raise(new ChangeHealthEvent(_currentHealth));
        }
    }
}
