using UnityEngine;
using Utils.StateMachine;

namespace Refactoring
{
    public class MissileState : IState
    {
        protected Transform _client;
        protected IMissileBaseData _data;

        public MissileState(Transform client, IMissileBaseData data)
        {
            _client = client;
            _data = data;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void FixedUpdate() { }
        public virtual void Update() { }
    }
}
