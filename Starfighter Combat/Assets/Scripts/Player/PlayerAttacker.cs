using UnityEngine;

public class PlayerAttacker : IAttacker
{
    protected readonly Player _player;
    private Transform _projectileSpawnPoint;
    protected readonly Timer _reloadTimer;

    protected bool _isShooted = false;

    public PlayerAttacker(Player player)
    {
        _player = player;
        _projectileSpawnPoint = _player.transform.Find("ProjectilePosition");

        _reloadTimer = new Timer(_player);
        _reloadTimer.TimeIsOver += OnReloadTimerExpired;
    }

    public virtual void Fire(GameObject projectile)
    {
        GameObject newProjectile;

        if (!_isShooted)
        {
            _isShooted = true;

            newProjectile = ObjectPoolManager.SpawnObject(projectile, _projectileSpawnPoint.transform.position,
               _projectileSpawnPoint.transform.rotation, ObjectPoolManager.PoolType.Weapon);
            RegistrProjectile(newProjectile);

            _reloadTimer.SetTimer(_player.PlayerData.ReloadTime);
            _reloadTimer.StartTimer();
        }
    }

    public void Reset()
    {
        _reloadTimer.StopTimer();
        _isShooted = false;
    }

    protected void RegistrProjectile(GameObject projectile)
    {

        EntryPoint.Instance.SpawnedObjects.RegisterObject(projectile, ObjectTag.PlayerWeapon);

    }

    protected void OnReloadTimerExpired() => _isShooted = false;
}
