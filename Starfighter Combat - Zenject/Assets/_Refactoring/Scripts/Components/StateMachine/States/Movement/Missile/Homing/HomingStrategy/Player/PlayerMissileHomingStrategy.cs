using System.Collections.Generic;
using UnityEngine;

namespace Refactoring
{
    public class PlayerMissileHomingStrategy : HomingStrategy
    {
        private Collider2D[] _missileTargets;
        private Queue<Transform> _targetsQueue;
        private CollisionMap _collisionMap;

        private LayerMask _layerMask;

        private const string _enemyLayerPath = "Enemy";

        public PlayerMissileHomingStrategy(Transform client, IMoveComponent missileMoveHandler, IDirectionReciever directionSetter, CollisionMap collisionMap) : base(client, missileMoveHandler, directionSetter)
        {
            _collisionMap = collisionMap;

            _layerMask = LayerMask.GetMask(_enemyLayerPath);

            _missileTargets = new Collider2D[35];
            _targetsQueue = new Queue<Transform>();
        }

        public override void StartHoming()
        {
            LockOnTarget();
            SeekNearestEnemy();

            base.StartHoming();
        }

        private void LockOnTarget()
        {
            var targets = Physics2D.OverlapCircleNonAlloc(_client.position, 25f, _missileTargets, _layerMask);

            for (int i = 0; i < targets; i++)
            {
                if (_collisionMap.PlayerMissileTargets.Contains(_missileTargets[i].transform))
                {
                    _targetsQueue.Enqueue(_missileTargets[i].transform);
                }
            }
        }

        private void SeekNearestEnemy()
        {
            Transform nearestEnemy = null;

            var nearestEnemyDistance = Mathf.Infinity;

            var count = _targetsQueue.Count;

            for (int i = 0; i < count; i++)
            {
                var target = _targetsQueue.Dequeue();
                var currentDistance = Vector2.Distance(_client.position, target.position);

                if (currentDistance < nearestEnemyDistance)
                {
                    nearestEnemy = target;
                    nearestEnemyDistance = currentDistance;
                }
            }

            _target = nearestEnemy;
        }
    }
}
