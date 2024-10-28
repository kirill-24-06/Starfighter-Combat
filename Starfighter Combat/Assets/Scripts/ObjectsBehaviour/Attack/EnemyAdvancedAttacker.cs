using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAdvancedAttacker : IAttacker
{
    private MonoBehaviour _client;
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
        _client = client;
        _audioPlayer = _client.GetComponentInChildren<AudioSource>();

        _shooterData = shooterData;
        _shotsPerAttackRun = shotsPerAttackRun;

        _firePoints = new List<Transform>();
        FindFirePoints(_client.transform);

        _reloadTimer = new Timer(_client);
        _reloadTimer.TimeIsOver += OnReloadTimerExpired;

    }

    public void Fire(GameObject projectile)
    {
        if (_isShooted) return;

        _isShooted = true;

        for (int i = 0; i < _firePoints.Count; i++)
        {
            ObjectPoolManager.SpawnObject(projectile, _firePoints[i].position, _firePoints[i].rotation, ObjectPoolManager.PoolType.Weapon);
        }

        _shotsFired++;
        _audioPlayer.PlayOneShot(_shooterData.FireSound, _shooterData.FireSoundVolume);

        if (_shotsFired >= _shotsPerAttackRun)
        {
            AttackRunComplete?.Invoke();
            _shotsFired = 0;
        }

        Reload();
    }

    private void Reload()
    {
        if (!_client.gameObject.activeInHierarchy) return;

        _reloadTimer.SetTimer(_shooterData.ReloadCountDown);
        _reloadTimer.StartTimer();
    }

    public void Reset()
    {
        _reloadTimer.StopTimer();
        _isShooted = false;
        _shotsFired = 0;
    }

    protected void OnReloadTimerExpired() => _isShooted = false;

    private void FindFirePoints(Transform client)
    {
        foreach (Transform child in client)
        {
            if (child.name == "FirePoint")
                _firePoints.Add(child);
        }
    }
}
