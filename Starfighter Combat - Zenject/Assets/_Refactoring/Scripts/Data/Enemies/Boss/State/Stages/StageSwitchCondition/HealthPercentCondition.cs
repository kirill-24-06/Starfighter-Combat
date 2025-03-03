using Utils.StateMachine;

namespace Refactoring
{
    public class HealthPercentCondition : IPredicate
    {
        private IReadOnlyProperty<int> _currentHealth;

        private int _maxHealth;

        private float _healthPercent;

        public HealthPercentCondition(IReadOnlyProperty<int> currentHealth, int maxHealth, float healthPercent)
        {
            _currentHealth = currentHealth;
            _maxHealth = maxHealth;
            _healthPercent = healthPercent;
        }

        public bool Evaluate()
        {
            return GlobalConstants.FloatConverter * _currentHealth.Value / _maxHealth <= _healthPercent;
        }
    }
}