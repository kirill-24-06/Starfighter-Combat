using UnityEngine;

namespace Refactoring
{
    public class MissileLaunchState : MissileState
    {
        private Timer _launchTimer;
        private IMoveComponent _basicMoveHandler;

        public MissileLaunchState(Transform client, IMissileBaseData data, IMoveComponent basicMoveComponent, Timer launchTimer) : base(client, data)
        {
            _basicMoveHandler = basicMoveComponent;
            _launchTimer = launchTimer;

            _launchTimer.TimeIsOver += () => IsLaunched = true;
        }

        public bool IsLaunched { get; private set; }

        public override void Enter()
        {
            IsLaunched = false;

            _launchTimer.SetTimer(_data.LaunchTime);
            _launchTimer.StartTimer();
        }

        public override void Update() => _basicMoveHandler.Move();
    }
}
