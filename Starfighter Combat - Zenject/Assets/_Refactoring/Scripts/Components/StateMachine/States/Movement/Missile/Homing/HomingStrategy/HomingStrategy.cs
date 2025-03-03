using System;
using UnityEngine;

namespace Refactoring
{
    public abstract class HomingStrategy : IHomingStrategy
    {
        protected Transform _client;
        protected Transform _target;

        protected IMoveComponent _missileMoveHandler;
        protected IDirectionReciever _directionSetter;

        public event Action<Transform> OnHoming = delegate { };

        public HomingStrategy(Transform client, IMoveComponent missileMoveHandler, IDirectionReciever directionSetter)
        {
            _client = client;
            _missileMoveHandler = missileMoveHandler;
            _directionSetter = directionSetter;
        }

        public virtual void StartHoming()
        {
            OnHoming.Invoke(_target);
        }

        public virtual void MoveTowardsTarget()
        {
            var target = _target.position;

            _directionSetter.RecieveDirection(target);
            _missileMoveHandler.Move();

            LookInTargetDirection(target);
        }

        protected void LookInTargetDirection(Vector3 target)
        {
            float rotation = Mathf.Atan2(target.y - _client.transform.position.y, target.x - _client.transform.position.x) * Mathf.Rad2Deg - 90;
            _client.transform.rotation = Quaternion.Euler(_client.transform.rotation.eulerAngles.x, _client.transform.rotation.eulerAngles.y, rotation);
        }
    }
}
