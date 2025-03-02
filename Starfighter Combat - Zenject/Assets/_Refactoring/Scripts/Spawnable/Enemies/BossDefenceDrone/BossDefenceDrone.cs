using System.Collections.Generic;
using System.Threading;
using Utils.Events.Channel.Static;
using Utils.StateMachine;
using Utils.SpawnSystem;
using UnityEngine;

namespace Refactoring
{
    public class BossDefenceDrone : Enemy
    {
        private BossDefenceDroneSettings _data;
        private StateMachine _stateMachine;

        public void Construct(BossDefenceDroneSettings settings, StateMachine stateMachine,
            EnemyHealthHandler enemyHealth, List<IResetable> resetables,
            IFactory<MonoProduct> explosionEffectsFactory, Player player,
            CollisionMap collisionMap, CancellationToken cancellationToken)
        {
            _data = settings;
            _enemyData = _data;

            _stateMachine = stateMachine;
            _damageHandler = enemyHealth;
            enemyHealth.Dead += OnDead;

            _enemyDestroyed = new EnemyDestroyedEvent(_data.EnemyStrenght);
            _addScore = new AddScoreEvent(_data.Score);
            _explosionSound = new PlayGlobalSoundEvent(_data.ExplosionSound, _data.ExplosionSoundVolume);

            _resetables = resetables;

            _explosionEffectsFactory = explosionEffectsFactory;
            _collisionMap = collisionMap;
            _player = player;

            _sceneExitToken = cancellationToken;

           IsInPool = false;

            IsConstructed = true;
        }

        private void FixedUpdate() => _stateMachine.OnFixedUpdate();

        private void Update() => _stateMachine.OnUpdate();

        protected override void Collide()
        {
            if (!_player.IsInvunerable && !_player.IsDroneActive)
                Channel<PlayerDamagedEvent>.Raise(_damage);

            else if (!_player.IsInvunerable && _player.IsDroneActive)
               Channel<DroneDestroyedEvent>.Raise(_destroyDrone);

            _damageHandler.TakeDamage(GlobalConstants.CollisionDamage);
        }

        public override void GetDamagedByNuke()
        {
            var reducedDamage = Mathf.RoundToInt(GlobalConstants.NukeDamage * _data.NukeResistModifier);

            _damageHandler.TakeDamage(reducedDamage);
        }

        protected override void OnDead()
        {
            if (IsInPool) return;
            IsInPool = true;

            CreateExplosion();

            Channel<AddScoreEvent>.Raise(_addScore);
            Channel<EnemyDestroyedEvent>.Raise(_enemyDestroyed);
            Channel<PlayGlobalSoundEvent>.Raise(_explosionSound);

            Release();
        }
    }
}