using UnityEngine;

public class EnemyAttacker : IAttacker
{
    private Transform _firePoint;

    protected readonly Timer _reloadTimer;
    protected IShooterData _shooterData;

    private AudioSource _audioPlayer;

    protected bool _isShooted = false;

    public EnemyAttacker(MonoBehaviour client, IShooterData shooterData)
    {
        _firePoint = client.transform.Find("FirePoint");
        _audioPlayer = client.GetComponentInChildren<AudioSource>();

        _shooterData = shooterData;

        _reloadTimer = new Timer(client);
        _reloadTimer.TimeIsOver += OnReloadTimerExpired;
    }

    public virtual void Fire(GameObject projectile)
    {
        GameObject newProjectile;

        if (!_isShooted)
        {
            _isShooted = true;

            newProjectile = ObjectPoolManager.SpawnObject(projectile, _firePoint.transform.position,
               _firePoint.transform.rotation, ObjectPoolManager.PoolType.Weapon);
            RegistrProjectile(newProjectile);

            _reloadTimer.SetTimer(_shooterData.ReloadCountDown);
            _reloadTimer.StartTimer();

            _audioPlayer.PlayOneShot(_shooterData.FireSound,_shooterData.FireSoundVolume);
        }
    }

    public void Reset()
    {
        _reloadTimer.StopTimer();
        _isShooted = false;
    }

    protected void RegistrProjectile(GameObject projectile)
    {
        EntryPoint.Instance.SpawnedObjects.RegisterObject(projectile, ObjectTag.EnemyWeapon);
    }

    protected void OnReloadTimerExpired() => _isShooted = false;
}