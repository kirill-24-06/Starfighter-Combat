using System.Collections.Generic;
using System.Threading;
using Utils.StateMachine;
using Utils.Events.Channel.Static;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class Interceptor : Enemy
    {
        private InterceptorSettings _data;
        private StateMachine _statemachine;
        private Timer _liveTimer;

        public void Construct(
            InterceptorSettings interceptorSettings,
            StateMachine stateMachine,
            EnemyHealthComponent enemyHealth,
            Timer timer,
            List<IResetable> resetables,
            IFactory<MonoProduct> explosionEffectsFactory,
            Player player,
            CollisionMap collisionMap,
            CancellationToken cancellationToken)
        {
            _data = interceptorSettings;
            _enemyData = _data;

            _statemachine = stateMachine;

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

        #region Monobehaviour
        private void FixedUpdate() => _statemachine.OnFixedUpdate();

        private void Update() => _statemachine.OnUpdate();
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
