using UnityEngine;

namespace Refactoring
{
    public class MoveState : EnemyMoveState
    {
        private IAdvancedMovableData _data;

        public MoveState(Transform client, IMoveComponent moveComponent, IAdvancedMovableData data) : base(client, moveComponent)
        {
            _data = data;
        }

        public bool IsEnteredGameArea => CheckArrival();

        private bool CheckArrival()
        {
            return (_client.position.x > -_data.GameArea.x || _client.position.x < _data.GameArea.x) && _client.position.y < _data.GameArea.y;
        }
    }

}
