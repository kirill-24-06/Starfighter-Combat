public class BossHealthHandler : IDamageble, IResetable
{
    private int _baseHealth;
    private int _currentHealth;

    public System.Action Dead;
    public System.Action<int> HealthChanged;

    private EventManager _events;

    public BossHealthHandler(int baseHealth, EventManager events)
    {
        _baseHealth = baseHealth;
        _currentHealth = _baseHealth;

        _events = events;
    }

    public void Reset() => _currentHealth = _baseHealth;

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
            return;

        _currentHealth -= damage;

        if (_currentHealth < 0)
            _currentHealth = 0;

        HealthChanged?.Invoke(_currentHealth);
        _events.BossDamaged?.Invoke(GlobalConstants.FloatConverter * _currentHealth / _baseHealth);

        if (_currentHealth == 0)
            Dead?.Invoke();
    }
}