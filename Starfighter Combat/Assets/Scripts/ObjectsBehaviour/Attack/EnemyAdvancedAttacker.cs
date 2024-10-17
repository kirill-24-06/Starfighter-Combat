using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAdvancedAttacker : IAttacker
{
    private List<Transform> _firePoints;

    protected readonly Timer _reloadTimer;
    protected IShooterData _shooterData;

    private AudioSource _audioPlayer;

    protected bool _isShooted = false;

    private int _shotsFired = 0;
    private int _shotsPerAttackRun;

    public Action AttackRunComplete;

    public EnemyAdvancedAttacker(MonoBehaviour client, IShooterData shooterData, int shotsPerAttackRun)
    {
        _audioPlayer = client.GetComponentInChildren<AudioSource>();

        _shooterData = shooterData;
        _shotsPerAttackRun = shotsPerAttackRun;

        _firePoints = new List<Transform>();
        FindFirePoints(client.transform);

        _reloadTimer = new Timer(client);
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

            _reloadTimer.SetTimer(_shooterData.ReloadCountDown);
            _reloadTimer.StartTimer();

            _audioPlayer.PlayOneShot(_shooterData.FireSound, _shooterData.FireSoundVolume);

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

    private void FindFirePoints(Transform client)
    {
        foreach (Transform child in client)
        {
            if (child.name == "FirePoint")
            {
                _firePoints.Add(child);
            }
        }
    }
}
