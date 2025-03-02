using System;

public class EnemyHealthHandler : IDamageble, IResetable
{
    private int _baseHealth;
    private int _currentHealth;

    public event Action Dead;

    public EnemyHealthHandler(int baseHealth)
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

        if (_currentHealth == 0)
            Dead?.Invoke();
    }
}