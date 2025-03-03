using System.Collections.Generic;
using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class BossMissileLaunchAbility : IBossAbility
    {
        private List<Transform> _missilePoints;
        private IFactory<MonoProduct> _missileFactory;

        public BossMissileLaunchAbility(Transform bossTransform, IFactory<MonoProduct> missileFactory)
        {
            GetMissilePointsIn(bossTransform);
            _missileFactory = missileFactory;
        }

        public void Cast()
        {
            foreach (var missilePoint in _missilePoints)
            {
                var missile = _missileFactory.Create().transform;

                missile.SetLocalPositionAndRotation(missilePoint.position, missilePoint.rotation);
            }
        }

        private void GetMissilePointsIn(Transform bossTransform)
        {
            _missilePoints = new List<Transform>();

            foreach (Transform componet in bossTransform.transform)
            {
                if (componet.name == "MissilePoint")
                {
                    _missilePoints.Add(componet);
                }
            }
        }
    }
}