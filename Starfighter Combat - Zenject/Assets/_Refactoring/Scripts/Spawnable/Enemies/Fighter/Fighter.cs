using System.Collections.Generic;
using System.Threading;
using Utils.Events.Channel.Static;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class Fighter : Enemy
    {
        private FighterSettings _data;
        private IWeapon _weapon;

        public void Construct(FighterSettings fighterSettings,
            IMoveComponent moveHandler, EnemyHealthHandler enemyHealthHandler,
            IWeapon weapon, List<IResetable> resetables, Player player,
            IFactory<MonoProduct> explosionEffectsFactory, CollisionMap collisionMap, CancellationToken cancellationToken)
        {
            _data = fighterSettings;
            _enemyData = _data;

            _moveHandler = moveHandler;

            _damageHandler = enemyHealthHandler;
            enemyHealthHandler.Dead += OnDead;

            _weapon = weapon;

            _resetables = resetables;

            _enemyDestroyed = new EnemyDestroyedEvent(_data.EnemyStrenght);
            _addScore = new AddScoreEvent(_data.Score);
            _explosionSound = new PlayGlobalSoundEvent(_data.ExplosionSound, _data.ExplosionSoundVolume);

            _player = player;
            _explosionEffectsFactory = explosionEffectsFactory;
            _collisionMap = collisionMap;

            _sceneExitToken = cancellationToken;

            IsInPool = false;

            IsConstructed = true;
        }

        private void FixedUpdate() => _weapon.Attack();

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
