using Utils.StateMachine;

public class StateMachineReset : IResetable
{
    private StateMachine _stateMachine;
    private IState _baseState;

    public StateMachineReset(StateMachine stateMachine, IState baseState)
    {
        _stateMachine = stateMachine;
        _baseState = baseState;
    }

    public void Reset() => _stateMachine.SetState(_baseState);
    
}
