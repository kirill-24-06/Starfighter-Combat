using System.Collections.Generic;
using UnityEngine;

public class MultipleCanonsAttacker : Attacker
{
    private readonly List<Transform> _projectileSpawnPoints = new List<Transform>();
    public MultipleCanonsAttacker(ObjectBehaviour objectBehaviour) : base(objectBehaviour)
    {
        FindSpawnPoints();
    }

    public override void Fire(GameObject projectile)
    {
        GameObject newProjectile;

        if (!_isShooted)
        {
            _isShooted = true;

            if (_objectBehaviour.ObjectInfo.Tag == ObjectTag.Enemy)
            {
                EventManager.GetInstance().Fire?.Invoke(_objectBehaviour);
            }

            foreach (Transform position in _projectileSpawnPoints)
            {
                newProjectile = ObjectPoolManager.SpawnObject(projectile, position.position, position.rotation, ObjectPoolManager.PoolType.Weapon);
                RegistrProjectile(newProjectile);
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
    }
}
