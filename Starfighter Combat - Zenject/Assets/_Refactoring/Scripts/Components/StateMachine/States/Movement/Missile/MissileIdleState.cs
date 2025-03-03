using UnityEngine;

namespace Refactoring
{
    public class MissileIdleState : MissileState
    {
        private IMoveComponent _basicMoveHandler;

        public MissileIdleState(Transform client, IMissileBaseData data, IMoveComponent basicMoveComponent) : base(client, data)
        {
            _basicMoveHandler = basicMoveComponent;
        }

        public override void Update() => _basicMoveHandler.Move();
    }
}
