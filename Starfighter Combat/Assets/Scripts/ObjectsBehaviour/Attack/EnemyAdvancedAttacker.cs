using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAdvancedAttacker : IAttacker,IResetable
{
    private GameObject _client;
    private List<Transform> _firePoints;
    private GameObject _projectile;

    private AudioSource _audioPlayer;
    private AudioClip _fireSound;
    private float _fireSoundVolume;

    protected readonly Timer _reloadTimer;
    private float _reloadCd;

    protected bool _isShooted = false;

    private int _shotsFired = 0;
    private int _shotsPerAttackRun;

    public Action AttackRunComplete;

    public EnemyAdvancedAttacker(MonoBehaviour client, IShooterData shooterData, int shotsPerAttackRun)
    {
        _client = client.gameObject;
        _firePoints = new List<Transform>();
        FindFirePoints(_client.transform);
        _projectile = shooterData.Projectile;

        _audioPlayer = _client.GetComponentInChildren<AudioSource>();
        _fireSound = shooterData.FireSound;
        _fireSoundVolume = shooterData.FireSoundVolume;

        _shotsPerAttackRun = shotsPerAttackRun;

        _reloadTimer = new Timer(client);
        _reloadCd = shooterData.ReloadCountDown;
        _reloadTimer.TimeIsOver += OnReloadTimerExpired;
    }

    public void Fire()
    {
        if (_isShooted) return;

        _isShooted = true;

        for (int i = 0; i < _firePoints.Count; i++)
        {
            ObjectPool.Get(_projectile, _firePoints[i].position, _firePoints[i].rotation);
        }

        _shotsFired++;

        if (_shotsFired >= _shotsPerAttackRun)
        {
            AttackRunComplete?.Invoke();
            _shotsFired = 0;
        }

        _audioPlayer.PlayOneShot(_fireSound, _fireSoundVolume);
        Reload();
    }

    private void Reload()
    {
        if (!_client.gameObject.activeInHierarchy) return;

        _reloadTimer.SetTimer(_reloadCd);
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
