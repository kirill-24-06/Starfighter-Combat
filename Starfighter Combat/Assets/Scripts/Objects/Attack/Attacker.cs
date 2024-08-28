using UnityEngine;

public class Attacker : IAttacker
{
    protected readonly MonoBehaviour _objectBehaviour;
    private Transform _projectileSpawnPoint;
    protected readonly Timer _reloadTimer;

    protected bool _isShooted = false;

    public Attacker(MonoBehaviour objectBehaviour)
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

            _reloadTimer.SetTimer(3);
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
