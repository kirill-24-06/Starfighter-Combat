using UnityEngine;

public class EnemyAttacker : IAttacker
{
    private Transform _firePoint;
    private GameObject _client;

    protected readonly Timer _reloadTimer;
    protected IShooterData _shooterData;

    private AudioSource _audioPlayer;

    protected bool _isShooted = false;

    public EnemyAttacker(MonoBehaviour client, IShooterData shooterData)
    {
        _client = client.gameObject;
        _firePoint = _client.transform.Find("FirePoint");
        _audioPlayer = _client.GetComponentInChildren<AudioSource>();

        _shooterData = shooterData;

        _reloadTimer = new Timer(client);
        _reloadTimer.TimeIsOver += OnReloadTimerExpired;
    }

    public virtual void Fire(GameObject projectile)
    {
        if (_isShooted) return;

        _isShooted = true;

        ObjectPoolManager.SpawnObject(projectile, _firePoint.transform.position,
           _firePoint.transform.rotation, ObjectPoolManager.PoolType.Weapon);


        if (!_client.activeInHierarchy) return;

        _audioPlayer.PlayOneShot(_shooterData.FireSound, _shooterData.FireSoundVolume);

        Reload();
    }

    private void Reload()
    {
        _reloadTimer.SetTimer(_shooterData.ReloadCountDown);
        _reloadTimer.StartTimer();
    }

    public void Reset()
    {
        _reloadTimer.StopTimer();
        _isShooted = false;
    }

    protected void OnReloadTimerExpired() => _isShooted = false;
}