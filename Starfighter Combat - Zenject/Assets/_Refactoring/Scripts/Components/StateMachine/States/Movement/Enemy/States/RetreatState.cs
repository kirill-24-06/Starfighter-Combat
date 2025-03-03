using UnityEngine;

namespace Refactoring
{
    public class RetreatState : EnemyMoveState
    {
        public RetreatState(Transform client, IMoveComponent moveComponent) : base(client, moveComponent) { }
    }

}
