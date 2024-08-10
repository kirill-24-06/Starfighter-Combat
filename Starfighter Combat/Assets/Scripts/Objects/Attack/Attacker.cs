using UnityEngine;

public class Attacker : IAttacker
{
    protected readonly ObjectBehaviour _objectBehaviour;
    private Transform _projectileSpawnPoint;
    protected readonly Timer _reloadTimer;

    protected bool _isShooted = false;

    public Attacker(ObjectBehaviour objectBehaviour)
    {
        _objectBehaviour = objectBehaviour;
        _projectileSpawnPoint = _objectBehaviour.transform.Find("ProjectilePosition");

        _reloadTimer = new Timer(_objectBehaviour);
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

            _reloadTimer.SetTimer(_objectBehaviour.ObjectInfo.ReloadTime);
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
        if (_objectBehaviour.ObjectInfo.Tag == ObjectTag.Player)
        {
            ObjectHolder.GetInstance().RegisterObject(projectile, ObjectTag.PlayerWeapon);
        }

        else if (_objectBehaviour.ObjectInfo.Tag == ObjectTag.Enemy)
        {
            ObjectHolder.GetInstance().RegisterObject(projectile, ObjectTag.EnemyWeapon);
        }
    }

    protected void OnReloadTimerExpired() => _isShooted = false;
}
