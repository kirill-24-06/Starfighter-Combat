namespace Utils.StateMachine
{
    /// <summary>
    /// Implements IState interface for state machine
    /// </summary>
    public abstract class StateMachineComposit : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly IState _baseState;

        protected StateMachineComposit(StateMachine stateMachine, IState baseState)
        {
            _stateMachine = stateMachine;
            _baseState = baseState;
        }
        public virtual void Enter() => _stateMachine.SetState(_baseState);
        public virtual void Update() => _stateMachine.OnUpdate();
        public virtual void FixedUpdate() => _stateMachine.OnFixedUpdate();
        public virtual void Exit() => _stateMachine.ExitCurrentState();
    }
}
