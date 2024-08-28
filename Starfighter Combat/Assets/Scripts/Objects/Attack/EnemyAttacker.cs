using UnityEngine;

public class EnemyAttacker : IAttacker
{
    protected readonly Enemy _client;
    private Transform _firePoint;

    protected readonly Timer _reloadTimer;
    protected float _reload;

    protected bool _isShooted = false;

    public EnemyAttacker(Enemy client, float reloadCountDown)
    {
        _client = client;
        _firePoint = _client.transform.Find("FirePoint");

        _reload = reloadCountDown;

        _reloadTimer = new Timer(_client);
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

            _reloadTimer.SetTimer(_reload);
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
        EntryPoint.Instance.SpawnedObjects.RegisterObject(projectile, ObjectTag.EnemyWeapon);
    }

    protected void OnReloadTimerExpired() => _isShooted = false;
}