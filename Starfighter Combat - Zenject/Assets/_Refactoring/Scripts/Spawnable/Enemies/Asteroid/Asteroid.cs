using System.Collections.Generic;
using System.Threading;
using Utils.Events.Channel.Static;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class Asteroid : Enemy
    {
        private AsteroidSettings _data;

        public void Construct(AsteroidSettings data, IMoveComponent mover,
            EnemyHealthHandler enemyHealthHandler, List<IResetable> resetables,
            Player player, IFactory<MonoProduct> explosionEffectsFactory,
            CollisionMap collisionMap, CancellationToken cancellationToken)
        {
            _data = data;
            _enemyData = _data;

            _moveHandler = mover;

            _damageHandler = enemyHealthHandler;
            enemyHealthHandler.Dead += OnDead;

            _resetables = resetables;

            _enemyDestroyed = new EnemyDestroyedEvent(_data.EnemyStrenght);
            _addScore = new AddScoreEvent(_data.Score);
            _explosionSound = new PlayGlobalSoundEvent(_data.ExplosionSound, _data.ExplosionSoundVolume);

            _explosionEffectsFactory = explosionEffectsFactory;
            _collisionMap = collisionMap;
            _player = player;

            _sceneExitToken = cancellationToken;

            IsInPool = false;

            IsConstructed = true;
        }

        private void Update() => _moveHandler.Move();

        #region Enemy

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

        #endregion
    }
}