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
        if (!_isShooted)
        {
            _isShooted = true;

            ObjectPoolManager.SpawnObject(projectile, _projectileSpawnPoint.transform.position,
                _projectileSpawnPoint.transform.rotation, ObjectPoolManager.PoolType.Weapon);

            _reloadTimer.SetTimer(_objectBehaviour.ObjectInfo.ReloadTime);
            _reloadTimer.StartTimer();
        }
    }

    public void Reset()
    {
        _reloadTimer.StopTimer();
        _isShooted = false;
    }

    protected void OnReloadTimerExpired() => _isShooted = false;
}
