using UnityEngine;

namespace Refactoring
{
    public class RepositionState : EnemyCombatState
    {
        private IMoveComponent _moveHandler;

        private IRepositionStrategy _repsotionStrategy;

        protected Vector3 _moveDirection;

        private bool _rotationAllowed;

        public bool IsArrived => _client.position == _moveDirection;

        public RepositionState(Transform client, IMoveComponent moveComponent, IRepositionStrategy repositionStrategy, bool allowRotation = true) : base(client)
        {
            _moveHandler = moveComponent;
            _repsotionStrategy = repositionStrategy;
            _rotationAllowed = allowRotation;
        }

        public override void Enter()
        {
            _moveDirection = _repsotionStrategy.GetRepositionDirection();

            if (!_rotationAllowed) return;

            RotateTowardsTarget(_moveDirection);
        }

        public override void Update() => _moveHandler.Move();
    }
}
