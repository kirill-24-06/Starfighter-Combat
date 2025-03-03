using Utils.StateMachine;
using UnityEngine;

namespace Refactoring
{
    public abstract class EnemyMoveState : IState
    {
        protected Transform _client;
        protected IMoveComponent _moveHandler;

        public EnemyMoveState(Transform client, IMoveComponent moveHandler)
        {
            _client = client;
            _moveHandler = moveHandler;
        }

        #region IState

        public virtual void Enter() { }
        public virtual void FixedUpdate() { }
        public virtual void Update() => _moveHandler.Move();
        public virtual void Exit() { }

        #endregion
    }

}
