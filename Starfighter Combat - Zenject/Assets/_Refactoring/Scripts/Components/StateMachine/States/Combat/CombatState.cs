using Utils.StateMachine;

namespace Refactoring
{
    public class CombatState : StateMachineComposit
    {
        private Timer _timer;
        private ILifetimeSettings _data;

        public bool LiveTimeIsOver {  get; protected set; }

        public CombatState(StateMachine stateMachine, IState baseState,Timer timer ,ILifetimeSettings settings) : base(stateMachine, baseState)
        {
            _data = settings;
            _timer = timer;

            _timer.TimeIsOver += () => LiveTimeIsOver = true;
        }

        public override void Enter()
        {
            LiveTimeIsOver = false;

            _timer.SetTimer(_data.LifeTime);
            _timer.StartTimer();

            base.Enter();
        }
    }
}
