using Utils.StateMachine;

namespace Refactoring
{
    public abstract class Missile : Projectile
    {
        protected StateMachine _missileStateMachine;
        protected IResetable _stateMachine;

        private void OnEnable()
        {
            if (!IsConstructed) return;

            _stateMachine.Reset();

            _isPooled = false;
        }
      
        private void Update() => _missileStateMachine.OnUpdate();
        private void FixedUpdate() => _missileStateMachine.OnFixedUpdate();
    }
}
