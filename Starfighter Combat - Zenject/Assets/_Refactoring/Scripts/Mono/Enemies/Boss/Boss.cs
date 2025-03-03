using UnityEngine;
using Utils.Events.Channel.Static;
using Utils.StateMachine;

namespace Refactoring
{
    public abstract class Boss : Enemy
    {
        protected StateMachine _bossStateMachine;
        protected IBossData _bossData;

        protected override void Start()
        {
            _collisionMap.Register(_collider, this);
            _collisionMap.RegisterNukeInteractable(_collider, this);
            _collisionMap.RegisterMissileTarget(_transform);

            Channel<BossArrivalEvent>.Raise(new BossArrivalEvent());
        }

        private void Update() => _bossStateMachine.OnUpdate();

        private void FixedUpdate() => _bossStateMachine.OnFixedUpdate();

        protected override void Collide()
        {
            if (!_player.IsInvunerable && !_player.IsDroneActive)
                Channel<PlayerDamagedEvent>.Raise(_damage);

            else if (!_player.IsInvunerable && _player.IsDroneActive)
                Channel<DroneDestroyedEvent>.Raise(_destroyDrone);

            TakeDamage(GlobalConstants.CollisionDamage);
        }

        public override void Interact() => TakeDamage(GlobalConstants.CollisionDamage);

        public override void GetDamagedByNuke()
        {
            var reducedDamage = Mathf.RoundToInt(GlobalConstants.NukeDamage * _bossData.NukeResistModifier);

            TakeDamage(reducedDamage);
        }

        protected virtual void TakeDamage(int damage) => _damageHandler.TakeDamage(damage);

    }
}