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
            ObjectPool.Get(_missile, missilePoint.position, missilePoint.rotation);
        }
    }
}
