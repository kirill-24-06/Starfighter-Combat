using UnityEngine;
using Utils.SpawnSystem;
using Utils.StateMachine;

namespace Refactoring
{
    public class PlayerMissile : Missile
    {
        private PlayerMissileSettings _settings;

        public void Construct(
            PlayerMissileSettings settings,
            StateMachine missileStateMachine,
            IFactory<MonoProduct> explosionEffectFactory,
            IResetable stateMachineReset,
            CollisionMap collisionMap)
        {
            _settings = settings;
            _projectileBaseData = _settings;

            _missileStateMachine = missileStateMachine;
            _stateMachine = stateMachineReset;

            _explosionEffectFactory = explosionEffectFactory;
            _collisionEffectFactory = _explosionEffectFactory;

            _collisionMap = collisionMap;

            _isPooled = false;

            IsConstructed = true;
        }

        protected override void Collide(Collision2D collision)
        {
            if (_collisionMap.Interactables.TryGetValue(collision.collider, out var enemy))
            {
                enemy.Interact();
            }
        }
    }
}
