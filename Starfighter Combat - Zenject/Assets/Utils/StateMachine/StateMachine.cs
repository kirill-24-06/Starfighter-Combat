using System;
using System.Collections.Generic;

namespace Utils.StateMachine
{
    public class StateMachine
    {
        private StateNode _current;

        private readonly Dictionary<Type, StateNode> _states = new();

        public void AddTransition(IState from, IState to, IPredicate condition) => GetOrAdd(from).AddTransition(to, condition);

        public void AddState(IState state) => GetOrAdd(state);

        public void OnUpdate()
        {
            var transition = GetTransition();

            if (transition != null)
                SwitchState(transition.To);

            _current.State?.Update();
        }

        public void OnFixedUpdate() => _current.State?.FixedUpdate();

        public void SetState(IState state)
        {
            _current = _states[state.GetType()];

            _current.State?.Enter();
        }

        public void ExitCurrentState() => _current.State?.Exit();

        private ITransition GetTransition()
        {
            foreach (var transition in _current.Transitions)
            {
                if (transition.Condition.Evaluate())
                    return transition;
            }

            return null;
        }

        private void SwitchState(IState newState)
        {
            if (newState == _current.State) return;

            _current.State?.Exit();
            _current = _states[newState.GetType()];
            _current.State.Enter();
        }

        private StateNode GetOrAdd(IState state)
        {
            var node = _states.GetValueOrDefault(state.GetType());

            if (node == null)
            {
                node = new StateNode(state);
                _states.Add(state.GetType(), node);
            }

            return node;
        }
    }
}
