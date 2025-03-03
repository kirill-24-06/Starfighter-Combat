using Utils.Pool.Generic;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class PlayerLaserFactory : GenericFactory<PlayerLaser, PlayerLaserSettings>
    {
        private CollisionMap _collisionMap;

        private IFactory<MonoProduct> _collisionEffectFactory;
        private IFactory<MonoProduct> _explosionEffectFactory;

        public PlayerLaserFactory(
            PlayerLaserSettings settings,
            [Zenject.Inject(Id = "CollisionEffect")] IFactory<MonoProduct> collisionEffectFactory,
            [Zenject.Inject(Id = "ExplosionEffect")] IFactory<MonoProduct> explosionEffectFactory,
            CollisionMap collisionMap)
        {
            _settings = settings;

            _collisionMap = collisionMap;

            _collisionEffectFactory = collisionEffectFactory;
            _explosionEffectFactory = explosionEffectFactory;

            _pool = new CustomPool<PlayerLaser>(
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

        protected override MonoProduct Build(PlayerLaser product)
        {
            var moveComponent = new BasicMoveComponent(product.transform, _settings);

            product.Construct(_settings, moveComponent, _collisionEffectFactory, _explosionEffectFactory, _collisionMap);

            return product;
        }
    }
}
