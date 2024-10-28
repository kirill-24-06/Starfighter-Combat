using System.Collections.Generic;
using UnityEngine;

public class MissileTargets
{
    private List<Transform> _playerMissileTartgets;

    public List<Transform> PlayerMissileTargets => _playerMissileTartgets;

    public MissileTargets()
    {
        _playerMissileTartgets = new List<Transform>();
    }

    public void AddEnemy(Transform enemyTransform) => _playerMissileTartgets.Add(enemyTransform);

    //public void RemoveEnemy(Transform enemyTransform)
    //{
    //    for (int i = 0; i < _playerMissileTartgets.Count; i++)
    //    {
    //        if (enemyTransform == _playerMissileTartgets[i])
    //        {
    //            _playerMissileTartgets.RemoveAt(i);

    //            break;
    //        }
    //    }
    //}
}