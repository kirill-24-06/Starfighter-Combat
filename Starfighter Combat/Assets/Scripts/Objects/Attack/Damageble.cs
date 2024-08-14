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

            if (_objectBehaviuor.ObjectInfo.Tag == ObjectTag.Player) { EventManager.GetInstance().PlayerDamaged?.Invoke(_currentHealth); }

            if (_currentHealth < 0) { _currentHealth = 0; }
            
        }

        if (_currentHealth == 0)
        {
            if (_objectBehaviuor.ObjectInfo.Tag == ObjectTag.Player)
            {
                EventManager.GetInstance().PlayerDied?.Invoke();
            }

            else
            {
                EventManager.GetInstance().AddScore?.Invoke(_objectBehaviuor.ObjectInfo.Score);
                ObjectPoolManager.ReturnObjectToPool(_objectBehaviuor.gameObject);
            }
        }
    }

    public void RecieveHealth(int incomingHealth)
    {
        if (incomingHealth >= 0 && _currentHealth + incomingHealth <= _maxHealth)
        {
            _currentHealth += incomingHealth;
            EventManager.GetInstance().PlayerHealed?.Invoke(_currentHealth);
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