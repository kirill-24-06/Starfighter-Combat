using UnityEngine;

public class EnemyAttacker : IAttacker,IResetable
{
    private GameObject _client;
    private Transform _firePoint;
    private GameObject _projectile;

    private AudioSource _audioPlayer;
    private AudioClip _fireSound;
    private float _fireSoundVolume;

    protected readonly Timer _reloadTimer;
    private float _reloadCd;

    protected bool _isOnReload = false;

    public EnemyAttacker(MonoBehaviour client, IShooterData shooterData)
    {
        _client = client.gameObject;
        _firePoint = _client.transform.Find("FirePoint");
        _projectile = shooterData.Projectile;

        _audioPlayer = _client.GetComponentInChildren<AudioSource>();
        _fireSound = shooterData.FireSound;
        _fireSoundVolume = shooterData.FireSoundVolume;

        _reloadTimer = new Timer(client);
        _reloadCd = shooterData.ReloadCountDown;
        _reloadTimer.TimeIsOver += OnReloadTimerExpired;
    }

    public virtual void Fire()
    {
        if (_isOnReload) return;

        _isOnReload = true;

        ObjectPool.Get(_projectile, _firePoint.position, _firePoint.rotation);


        if (!_client.activeInHierarchy) return;

        _audioPlayer.PlayOneShot(_fireSound, _fireSoundVolume);

        Reload();
    }

    private void Reload()
    {
        _reloadTimer.SetTimer(_reloadCd);
        _reloadTimer.StartTimer();
    }

    public void Reset()
    {
        _reloadTimer.StopTimer();
        _isOnReload = false;
    }

    protected void OnReloadTimerExpired() => _isOnReload = false;
}