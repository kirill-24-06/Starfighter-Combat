
public class EnemyAce : Boss
{
    private BossShieldBehaviour _bossShield;
    protected override void Initialise()
    {
        base.Initialise();

        _bossShield = transform.Find("BossShield").gameObject.GetComponent<BossShieldBehaviour>();
        EntryPoint.Instance.Events.EnemyDamaged += _bossShield.OnDamage;
    }

    private void Start()
    {
        EntryPoint.Instance.SpawnedObjects.RegisterObject(gameObject, ObjectTag.Enemy);
        EntryPoint.Instance.SpawnedObjects.RegisterObject(_bossShield.gameObject, ObjectTag.Enemy);
        EntryPoint.Instance.SpawnedObjects.RegisterObject(transform.Find("EmergencyShield").gameObject, ObjectTag.Enemy);
        EntryPoint.Instance.Events.BossArrival?.Invoke();
    }

    protected override void TakeDamage(int damage)
    {
        if (damage <= 0 || _bossShield.IsActive || _isInvunerable)
            return;

        _currentHealth -= damage;

        if (_currentHealth < 0) _currentHealth = 0;

        _events.BossDamaged?.Invoke(GlobalConstants.FloatConverter * _currentHealth / _data.MaxHealth);

        if (_currentHealth == 0) Disable();
    }


    protected override void Disable()
    {
        Instantiate(_data.Explosion, transform.position, _data.Explosion.transform.rotation);

        EntryPoint.Instance.Events.BossDefeated?.Invoke();
        gameObject.SetActive(false);

        _audioPlayer.PlayOneShot(_data.ExplosionSound, _data.ExplosionSoundVolume);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        EntryPoint.Instance.Events.EnemyDamaged -= _bossShield.OnDamage;
    }
}