using Utils.StateMachine;

namespace Refactoring
{
    public class InterceptorStage : StateMachineComposit, IBossStage
    {
        protected IPredicate _stageCompleteCondition;

        public bool StageCompleted => _stageCompleteCondition.Evaluate();

        public InterceptorStage(IPredicate completeCondition, StateMachine stateMachine, IState baseState) : base(stateMachine, baseState)
        {
            _stageCompleteCondition = completeCondition;
        }
    }
}