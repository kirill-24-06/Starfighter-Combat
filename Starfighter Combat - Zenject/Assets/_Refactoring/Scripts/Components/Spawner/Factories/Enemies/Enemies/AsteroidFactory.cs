using System.Threading;
using Utils.SpawnSystem;
using Utils.Pool.Generic;
using System.Collections.Generic;

namespace Refactoring
{
    public class AsteroidFactory : GenericFactory<Asteroid, AsteroidSettings>
    {
        private Player _player;
        private CollisionMap _collisionMap;
        private IFactory<MonoProduct> _explosionEffectsFactory;
        private CancellationToken _cancellationToken;

        public AsteroidFactory(AsteroidSettings asteroidSettings, IFactory<MonoProduct> explosionEffectsFactory, Player player, CollisionMap collisionMap, CancellationToken cancellationToken)
        {
            _settings = asteroidSettings;
            _explosionEffectsFactory = explosionEffectsFactory;
            _player = player;
            _collisionMap = collisionMap;

            _pool = new CustomPool<Asteroid>(
                _settings.Create,
                _settings.OnGet,
                _settings.OnRelease,
                _settings.PrewarmAmount);

            _cancellationToken = cancellationToken;
        }

        public override MonoProduct Create()
        {
            var product = _pool.Get();

            return !product.IsConstructed ? Build(product).WithRelease(Release) : product;
        }

        protected override MonoProduct Build(Asteroid product)
        {
            var mover = new BasicMoveComponent(product.transform, _settings);
            var health = new EnemyHealthComponent(_settings.Health);

            var resetables = new List<IResetable>
            {
                health
            };

            product.Construct(_settings, mover, health, resetables, _player, _explosionEffectsFactory, _collisionMap, _cancellationToken);

            return product;
        }
    }
}

