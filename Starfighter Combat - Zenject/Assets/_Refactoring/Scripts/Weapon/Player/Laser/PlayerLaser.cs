using UnityEngine;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class PlayerLaser : Projectile
    {
        private PlayerLaserSettings _settings;
        private IMoveComponent _moveComponent;

        public void Construct(
            PlayerLaserSettings settings,
            IMoveComponent moveComponent,
            IFactory<MonoProduct> collisionEffectFactory,
            IFactory<MonoProduct> explosionEffectFactory,
            CollisionMap collisionMap)
        {
            _settings = settings;
            _projectileBaseData = _settings;

            _moveComponent = moveComponent;

            _collisionEffectFactory = collisionEffectFactory;
            _explosionEffectFactory = explosionEffectFactory;

            _collisionMap = collisionMap;

            _isPooled = false;

            IsConstructed = true;
        }

        private void Update() => _moveComponent.Move();

        protected override void Collide(Collision2D collision)
        {
            if (_collisionMap.Interactables.TryGetValue(collision.collider, out var enemy))
            {
                enemy.Interact();
            }
        }
    }
}
