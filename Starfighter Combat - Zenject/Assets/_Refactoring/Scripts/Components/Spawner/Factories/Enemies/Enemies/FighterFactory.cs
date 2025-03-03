using System.Collections.Generic;
using System.Threading;
using Utils.SpawnSystem;
using Utils.Pool.Generic;

namespace Refactoring
{
    public class FighterFactory : GenericFactory<Fighter, FighterSettings>
    {
        private IFactory<MonoProduct> _effectFactory;
        private IFactory<MonoProduct> _projectileFactory;

        private Player _player;
        private CollisionMap _collisionMap;

        private CancellationToken _cancellationToken;

        public FighterFactory(
            FighterSettings fighterSettings,
            IFactory<MonoProduct> visualEffectFactory,
            [Zenject.Inject(Id = "EnemyLaser")] IFactory<MonoProduct> projectileFactory,
            Player player,
            CollisionMap collisionMap,
            CancellationToken cancellationToken)
        {
            _settings = fighterSettings;

            _effectFactory = visualEffectFactory;
            _projectileFactory = projectileFactory;

            _player = player;
            _collisionMap = collisionMap;
            _cancellationToken = cancellationToken;

            _pool = new CustomPool<Fighter>(
                _settings.Create,
                _settings.OnGet,
                _settings.OnRelease,
                _settings.PrewarmAmount);
        }

        public override MonoProduct Create()
        {
            var product = _pool.Get();

            return !product.IsConstructed ? Build(product).WithRelease(Release) : product;
        }

        protected override MonoProduct Build(Fighter product)
        {
            var resetables = new List<IResetable>();

            var mover = new BasicMoveComponent(product.transform, _settings);

            var health = new EnemyHealthComponent(_settings.Health);
            resetables.Add(health);

            var weapon = new EnemyCannon(product, _projectileFactory, _settings);
            resetables.Add(weapon);

            product.Construct(
                _settings, mover, health, weapon, resetables, _player, _effectFactory, _collisionMap, _cancellationToken);

            return product;
        }
    }
}
