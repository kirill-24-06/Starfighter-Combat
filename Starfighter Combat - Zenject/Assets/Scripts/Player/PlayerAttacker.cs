using UnityEngine;

public class PlayerAttacker : IAttacker, IResetable
{
    private Transform _firePoint;
    protected GameObject _projectile;
    protected AudioSource _playerAudio;

    protected AudioClip _fireSound;
    protected float _fireSoundVolume;

    protected readonly Timer _reloadTimer;
    protected float _reloadTime;

    protected bool _isShooted = false;

    public PlayerAttacker(Transform player,Timer timer ,IShooterData data)
    {
        _firePoint = player.Find("ProjectilePosition");
        _projectile = data.Projectile;

        _playerAudio = player.GetComponentInChildren<AudioSource>();

        _reloadTimer = timer;
        _reloadTime = data.ReloadCountDown;
        _reloadTimer.TimeIsOver += OnReloadTimerExpired;

        _fireSound = data.FireSound;
        _fireSoundVolume = data.FireSoundVolume;

    }

    public virtual void Fire()
    {
        if (_isShooted) return;
        _isShooted = true;

        ObjectPool.Get(_projectile, _firePoint.position, _firePoint.rotation);

        _playerAudio.PlayOneShot(_fireSound, _fireSoundVolume);

        Reload();
    }

    private void Reload()
    {
        _reloadTimer.SetTimer(_reloadTime);
        _reloadTimer.StartTimer();
    }

    public void Reset()
    {
        _reloadTimer.StopTimer();
        _isShooted = false;
    }

    protected void OnReloadTimerExpired() => _isShooted = false;
}
