using UnityEngine;

public class Damageble : IDamageble
{
    ObjectBehaviour _objectBehaviuor;

    private int _baseHealth;
    private int _maxHealth;
    private int _currentHealth;

    public Damageble(ObjectBehaviour objectBehaviour)
    {
        _objectBehaviuor = objectBehaviour;

        _baseHealth = _objectBehaviuor.ObjectInfo.Health;
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
            if (_objectBehaviuor.gameObject.CompareTag("Player"))
            {
                Debug.Log("GAMEOVER");
                Time.timeScale = 0;
            }
            else
            {
                ObjectPoolManager.ReturnObjectToPool(_objectBehaviuor.gameObject);
                Debug.Log("Kill");
            }
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