using Utils.Pool.Generic;
using Utils.SpawnSystem;

namespace Refactoring
{
    public class EnemyLaserFactory : GenericFactory<EnemyLaser, EnemyLaserSettings>
    {
        private CollisionMap _collisionMap;
        private IFactory<MonoProduct> _collisionEffectFactory;
        private IFactory<MonoProduct> _explosionEffectFactory;

        private Player _player;

        public EnemyLaserFactory(
            EnemyLaserSettings settings,
            CollisionMap collisionMap,
            [Zenject.Inject(Id = "CollisionEffect")] IFactory<MonoProduct> collisionEffectFactory,
            [Zenject.Inject(Id = "ExplosionEffect")] IFactory<MonoProduct> explosionEffectFactory,
            Player player)
        {
            _settings = settings;

            _collisionMap = collisionMap;

            _collisionEffectFactory = collisionEffectFactory;
            _explosionEffectFactory = explosionEffectFactory;

            _player = player;

            _pool = new CustomPool<EnemyLaser>(
                _settings.Create,
                _settings.OnGet,
                _settings.OnRelease,
                _settings.PrewarmAmount);
        }

        public override MonoProduct Create()
        {
            var laser = _pool.Get();

            return !laser.IsConstructed ? Build(laser).WithRelease(Release) : laser;
        }

        protected override MonoProduct Build(EnemyLaser product)
        {
            var moveComponent = new BasicMoveComponent(product.transform, _settings);

            product.Construct(_settings, moveComponent,_collisionEffectFactory, _explosionEffectFactory, _player, _collisionMap);

            return product;
        }
    }
}
