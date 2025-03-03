using System.Collections.Generic;
using System.Threading;
using Utils.SpawnSystem;
using Utils.StateMachine;
using Utils.Events.Channel.Static;

namespace Refactoring
{
    public class RocketShip : Enemy
    {
        private RocketShipSettings _data;
        private StateMachine _stateMachine;
        private Timer _liveTimer;

        public void Construct(
            RocketShipSettings settings,
            StateMachine stateMachine,
            EnemyHealthComponent enemyHealth,
            Timer timer,
            List<IResetable> resetables,
            IFactory<MonoProduct> explosionEffectsFactory,
            Player player,
            CollisionMap collisionMap,
            CancellationToken cancellationToken)
        {

            _data = settings;
            _enemyData = settings;
            _stateMachine = stateMachine;

            _damageHandler = enemyHealth;
            enemyHealth.Dead += OnDead;

            _liveTimer = timer;

            _resetables = resetables;

            _explosionEffectsFactory = explosionEffectsFactory;
            _collisionMap = collisionMap;
            _player = player;

            _sceneExitToken = cancellationToken;

            _enemyDestroyed = new EnemyDestroyedEvent(_data.EnemyStrenght);
            _addScore = new AddScoreEvent(_data.Score);
            _explosionSound = new PlayGlobalSoundEvent(_data.ExplosionSound, _data.ExplosionSoundVolume);


            IsInPool = false;

            IsConstructed = true;
        }

        #region MonoBechaviour
        private void FixedUpdate() => _stateMachine.OnFixedUpdate();
        private void Update() => _stateMachine.OnUpdate();

        #endregion

        #region Enemy

        protected override void OnDead()
        {
            if (IsInPool) return;
            IsInPool = true;

            CreateExplosion();

            Channel<AddScoreEvent>.Raise(_addScore);
            Channel<EnemyDestroyedEvent>.Raise(_enemyDestroyed);

            Channel<PlayGlobalSoundEvent>.Raise(_explosionSound);

            _liveTimer.StopTimer();

            Release();
        }

        #endregion
    }
}
