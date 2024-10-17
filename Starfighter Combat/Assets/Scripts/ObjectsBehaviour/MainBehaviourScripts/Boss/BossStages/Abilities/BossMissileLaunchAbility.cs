using System.Collections.Generic;
using UnityEngine;

public class BossMissileLaunchAbility : IBossAbility
{
    private List<Transform> _missilePoints = new List<Transform>();
    private GameObject _missile;

    public void Initialise(Boss boss)
    {
        foreach (Transform missilePoint in boss.transform)
        {
            if (missilePoint.name == "MissilePoint")
            {
                _missilePoints.Add(missilePoint);
            }
        }

        _missile = (GameObject)Resources.Load("Prefabs/Weapon/BossMissile");
    }

    public void Cast()
    {
        foreach (var missilePoint in _missilePoints)
        {
            ObjectPoolManager.SpawnObject(_missile, missilePoint.position, missilePoint.rotation, ObjectPoolManager.PoolType.Weapon);
            EntryPoint.Instance.SpawnedObjects.RegisterObject(_missile, ObjectTag.EnemyWeapon);
        }
    }
}
