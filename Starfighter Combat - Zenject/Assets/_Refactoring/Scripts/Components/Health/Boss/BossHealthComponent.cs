using System;
using UnityEngine;
using Utils.Events.Channel.Static;

namespace Refactoring
{
    public class BossHealthComponent : IDamageble, IResetable, IReadOnlyProperty<int>
    {
        private int _baseHealth;
        private int _currentHealth;

        public int Value => _currentHealth;
        public event Action<int> OnValueChanged;

        public event Action Dead;

        public BossHealthComponent(int baseHealth)
        {
            _baseHealth = baseHealth;
            _currentHealth = _baseHealth;
        }

        public void Reset() => _currentHealth = _baseHealth;

        public void TakeDamage(int damage)
        {
            if (damage <= 0)
                return;

            _currentHealth -= damage;

            if (_currentHealth < 0)
                _currentHealth = 0;

            OnValueChanged?.Invoke(_currentHealth);

            var sliderValue = GlobalConstants.FloatConverter * _currentHealth / _baseHealth;
            Channel<BossDamagedEvent>.Raise(new BossDamagedEvent(sliderValue));

            if (_currentHealth == 0)
                Dead?.Invoke();
        }
    }
}