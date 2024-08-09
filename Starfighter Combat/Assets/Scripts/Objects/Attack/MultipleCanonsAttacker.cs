using System.Collections.Generic;
using UnityEngine;

public class MultipleCanonsAttacker : Attacker
{
    private List<Transform> _projectileSpawnPoints = new List<Transform>();
    public MultipleCanonsAttacker(ObjectBehaviour objectBehaviour) : base(objectBehaviour)
    {
        FindSpawnPoints();
    }

    public override void Fire(GameObject projectile)
    {
        if (!_isShooted)
        {
            _isShooted = true;

            foreach (Transform position in _projectileSpawnPoints)
            {
                ObjectPoolManager.SpawnObject(projectile, position.transform.position, position.transform.rotation, ObjectPoolManager.PoolType.Weapon);
            }

            _reloadTimer.SetTimer(_objectBehaviour.ObjectInfo.ReloadTime);
            _reloadTimer.StartTimer();
        }
    }

    private void FindSpawnPoints()
    {
        foreach (Transform child in _objectBehaviour.transform)
        {
            if (child.name == "ProjectilePosition")
            {
                _projectileSpawnPoints.Add(child);
            }
        }

        Debug.Log(_projectileSpawnPoints.Count);
    }
}
