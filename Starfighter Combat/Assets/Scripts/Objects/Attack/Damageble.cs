using UnityEngine;

public class Damageble : IDamageble
{
    private int _baseHealth;
    private int _maxHealth;
    private int _currentHealth;

    public Damageble(int health)
    {
        _baseHealth = health;
        ResetHealth();
    }

    public void TakeDamage(int damage)
    {
        if (damage >= 0)
        {
            _currentHealth -= damage;

            if (_currentHealth < 0)
            {
                _currentHealth = 0;
            }

            Debug.Log(_currentHealth);
        }

        if (_currentHealth == 0)
        {
            Debug.Log("Kill");
        }
    }

    public void RecieveHealth(int incomingHealth)
    {
        if (incomingHealth >= 0 && _currentHealth + incomingHealth <= _maxHealth)
        {
            _currentHealth += incomingHealth;
        }
    }

    public void IncreaseMaxHealth()
    {
        _maxHealth++;
    }

    public void ResetHealth()
    {
        _maxHealth = _baseHealth;
        _currentHealth = _baseHealth;
    }
}