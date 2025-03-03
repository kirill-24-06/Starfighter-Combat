using UnityEngine;
using Utils.Events.Channel.Static;
using Utils.SpawnSystem;
using Utils.StateMachine;

namespace Refactoring
{
    public class EnemyMissile : Missile
    {
        private EnemyMissileSettings _settings;

        private Player _player;
        private PlayerDamagedEvent _damage;
        private DroneDestroyedEvent _destroyDrone;

        public void Construct(
            EnemyMissileSettings settings,
            StateMachine missileStateMachine,
            IFactory<MonoProduct> explosionEffectFactory,
            IResetable stateMachineReset,
            Player player,
            CollisionMap collisionMap)
        {
            _settings = settings;
            _projectileBaseData = _settings;

            _missileStateMachine = missileStateMachine;
            _stateMachine = stateMachineReset;

            _explosionEffectFactory = explosionEffectFactory;
            _collisionEffectFactory = _explosionEffectFactory;

            _player = player;

            _collisionMap = collisionMap;

            _damage = new PlayerDamagedEvent(GlobalConstants.CollisionDamage);
            _destroyDrone = new DroneDestroyedEvent();

            _isPooled = false;

            IsConstructed = true;
        }

        protected override void Collide(Collision2D collision)
        {
            if (!_player.IsInvunerable && _player.IsDroneActive)
            {
                Channel<DroneDestroyedEvent>.Raise(_destroyDrone);
            }

            else if (!_player.IsInvunerable && !_player.IsDroneActive)
            {
                Channel<PlayerDamagedEvent>.Raise(_damage);
            }
        }
    }
}
