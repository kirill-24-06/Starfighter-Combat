using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackerMultiple : PlayerAttacker
{
    private readonly List<Transform> _firePoints;

    public PlayerAttackerMultiple(Transform player, Timer timer, IShooterData data) : base(player, timer, data)
    {
        _firePoints = FindSpawnPoints(player);
    }

    public override void Fire()
    {
        if (!_isShooted)
        {
            _isShooted = true;

            for (int i = 0; i < _firePoints.Count; i++)
                ObjectPool.Get(_projectile, _firePoints[i].position, _firePoints[i].rotation);

            _playerAudio.PlayOneShot(_fireSound, _fireSoundVolume);

            Reload();
        }
    }

    private void Reload()
    {
        _reloadTimer.SetTimer(_reloadTime);
        _reloadTimer.StartTimer();
    }

    private List<Transform> FindSpawnPoints(Transform parrent)
    {
        var result = new List<Transform>();

        foreach (Transform child in parrent)
        {
            if (child.name == "ProjectilePosition")
                result.Add(child);
        }
        return result;
    }
}
