
using UnityEngine;

public class EnemyAce : Boss
{
    private BossShieldBehaviour _bossShield;
    protected override void Initialise()
    {
        base.Initialise();

        _bossShield = transform.Find("BossShield").gameObject.GetComponent<BossShieldBehaviour>();
    }

    private void Start()
    {
        var collider = GetComponent<PolygonCollider2D>();
        EntryPoint.Instance.CollisionMap.Register(collider, this);
        EntryPoint.Instance.CollisionMap.RegisterNukeInteractable(collider, this);
        EntryPoint.Instance.CollisionMap.RegisterMissileTarget(transform);
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
}