using UnityEngine;

namespace Refactoring
{
    public class MissileHomingState : MissileState
    {
        private Timer _homingTimer;
        private IHomingStrategy _homing;
        private GameObject _targetGO;
        public bool IsHoming { get; private set; }

        public MissileHomingState(Transform client, IMissileBaseData data, IHomingStrategy homingStrategy, Timer homingTimer) : base(client, data)
        {
            _homing = homingStrategy;
            _homingTimer = homingTimer;

            _homingTimer.TimeIsOver += () => IsHoming = false;
        }

        public override void Enter()
        {
            IsHoming = true;

            _homing.StartHoming();

            _homingTimer.SetTimer(_data.HomingTime);
            _homingTimer.StartTimer();
        }

        public override void Update()
        {
            if (!IsHoming) return;

            UpdateTarget();

            _homing.MoveTowardsTarget();
        }

        private void UpdateTarget()
        {
            if (_targetGO.activeInHierarchy) return;

            IsHoming = false;
        }

        public void OnHoming(Transform target)
        {
            if (target != null)
            {
                _targetGO = target.gameObject;
                return;
            }

            IsHoming = false;
        }
    }
}
