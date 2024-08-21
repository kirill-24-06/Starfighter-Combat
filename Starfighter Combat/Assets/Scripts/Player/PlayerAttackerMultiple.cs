using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackerMultiple : PlayerAttacker
{
    private readonly List<Transform> _projectileSpawnPoints = new List<Transform>();

    public PlayerAttackerMultiple(Player player) : base(player)
    {
        FindSpawnPoints();
    }

    public override void Fire(GameObject projectile)
    {
        GameObject newProjectile;

        if (!_isShooted)
        {
            _isShooted = true;

            foreach (Transform position in _projectileSpawnPoints)
            {
                newProjectile = ObjectPoolManager.SpawnObject(projectile, position.position, position.rotation, ObjectPoolManager.PoolType.Weapon);
                RegistrProjectile(newProjectile);
            }

            _reloadTimer.SetTimer(_player.PlayerData.ReloadTime);
            _reloadTimer.StartTimer();
        }
    }

    private void FindSpawnPoints()
    {
        foreach (Transform child in _player.transform)
        {
            if (child.name == "ProjectilePosition")
            {
                _projectileSpawnPoints.Add(child);
            }
        }
    }
}
