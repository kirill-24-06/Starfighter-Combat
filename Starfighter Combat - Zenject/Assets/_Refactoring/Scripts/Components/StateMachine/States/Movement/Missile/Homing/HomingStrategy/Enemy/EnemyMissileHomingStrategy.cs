using UnityEngine;

namespace Refactoring
{
    public class EnemyMissileHomingStrategy : HomingStrategy
    {
        public EnemyMissileHomingStrategy(Transform client, IMoveComponent missileMoveComponent, IDirectionReciever directionSetter, Transform target) : base(client, missileMoveComponent, directionSetter)
        {
            _target = target;
        }
    }
}
