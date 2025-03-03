using Utils.StateMachine;
using UnityEngine;

namespace Refactoring
{
    public abstract class EnemyCombatState : IState
    {
        protected Transform _client;

        public EnemyCombatState(Transform client)
        {
            _client = client;
        }

        #region IState
        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void Update()
        {
        }

        #endregion

        protected void RotateTowardsTarget(Vector3 target)
        {
            float rotation = Mathf.Atan2(target.y - _client.position.y, target.x - _client.position.x) * Mathf.Rad2Deg - 90;
            _client.rotation = Quaternion.Euler(_client.rotation.eulerAngles.x, _client.rotation.eulerAngles.y, rotation);
        }
    }

   
}
