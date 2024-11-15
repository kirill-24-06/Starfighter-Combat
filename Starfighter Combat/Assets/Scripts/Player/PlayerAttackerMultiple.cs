using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackerMultiple : PlayerAttacker
{
    private readonly List<Transform> _firePoints;

    public PlayerAttackerMultiple(Player player) : base(player)
    {
        _firePoints = new List<Transform>();
        FindSpawnPoints();
    }

    public override void Fire()
    {
        if (!_isShooted)
        {
            _isShooted = true;

            for (int i = 0; i < _firePoints.Count; i++)
                ObjectPool.Get(_projectile, _firePoints[i].position, _firePoints[i].rotation);

            _playerAudio.PlayOneShot(_player.PlayerData.FireSound, _player.PlayerData.FireSoundVolume);

            Reload();
        }
    }

    private void Reload()
    {
        if (!_player.gameObject.activeInHierarchy) return;

        _reloadTimer.SetTimer(_player.PlayerData.ReloadTime);
        _reloadTimer.StartTimer();
    }

    private void FindSpawnPoints()
    {
        foreach (Transform child in _player.transform)
        {
            if (child.name == "ProjectilePosition")
                _firePoints.Add(child);
        }
    }
}
