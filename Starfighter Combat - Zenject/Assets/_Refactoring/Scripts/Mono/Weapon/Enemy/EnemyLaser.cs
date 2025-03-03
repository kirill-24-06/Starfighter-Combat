using UnityEngine;
using Utils.Events.Channel.Static;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class EnemyLaser : Projectile
    {
        private EnemyLaserSettings _settings;

        private IMoveComponent _moveComponent;

        private Player _player;
        private PlayerDamagedEvent _damage;
        private DroneDestroyedEvent _destroyDrone;

        public void Construct(
            EnemyLaserSettings settings,
            IMoveComponent moveComponent,
            IFactory<MonoProduct> collisionEffectFactory,
            IFactory<MonoProduct> explosionEffectFactory,
            Player player ,
            CollisionMap collisionMap)
        {
            _settings = settings;
            _projectileBaseData = _settings;

            _moveComponent = moveComponent;

            _collisionEffectFactory = collisionEffectFactory;
            _explosionEffectFactory = explosionEffectFactory;

            _player = player;

            _collisionMap = collisionMap;

            _damage = new PlayerDamagedEvent(GlobalConstants.CollisionDamage);
            _destroyDrone = new DroneDestroyedEvent();

            _isPooled = false;

            IsConstructed = true;
        }

        private void Update() => _moveComponent.Move();

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
