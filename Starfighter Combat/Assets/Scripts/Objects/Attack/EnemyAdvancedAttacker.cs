using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAdvancedAttacker : IAttacker
{
    protected readonly Enemy _client;
    private List<Transform> _firePoints;

    protected readonly Timer _reloadTimer;
    protected float _reload;

    protected bool _isShooted = false;

    private int _shotsFired = 0;
    private int _shotsPerAttackRun;

    public Action AttackRunComplete;

    public EnemyAdvancedAttacker(Enemy client, float reloadCountDown, int shotsPerAttackRun)
    {
        _client = client;
        _reload = reloadCountDown;
        _shotsPerAttackRun = shotsPerAttackRun;

        _firePoints = new List<Transform>();
        FindFirePoints();

        _reloadTimer = new Timer(_client);
        _reloadTimer.TimeIsOver += OnReloadTimerExpired;

    }

    public void Fire(GameObject projectile)
    {
        GameObject newProjectile;

        if (!_isShooted)
        {
            _isShooted = true;

            foreach (Transform position in _firePoints)
            {
                newProjectile = ObjectPoolManager.SpawnObject(projectile, position.position, position.rotation, ObjectPoolManager.PoolType.Weapon);
                RegistrProjectile(newProjectile);
            }

            _reloadTimer.SetTimer(_reload);
            _reloadTimer.StartTimer();

            _shotsFired++;
        }

        if(_shotsFired >= _shotsPerAttackRun)
        {
            AttackRunComplete?.Invoke();
            _shotsFired = 0;
        }
    }

    public void Reset()
    {
        _reloadTimer.StopTimer();
        _isShooted = false;
        _shotsFired = 0;
    }

    protected void RegistrProjectile(GameObject projectile)
    {
        EntryPoint.Instance.SpawnedObjects.RegisterObject(projectile, ObjectTag.EnemyWeapon);
    }

    protected void OnReloadTimerExpired() => _isShooted = false;

    private void FindFirePoints()
    {
        foreach (Transform child in _client.transform)
        {
            if (child.name == "FirePoint")
            {
                _firePoints.Add(child);
            }
        }
    }
}
