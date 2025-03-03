using Utils.Pool.Generic;
using Utils.SpawnSystem;
using Utils.StateMachine;

namespace Refactoring
{
    public class PlayerMissileFactory : GenericFactory<PlayerMissile, PlayerMissileSettings>
    {
        private CollisionMap _collisionMap;
        private IFactory<MonoProduct> _explosionEffectFactory;

        public PlayerMissileFactory(
            PlayerMissileSettings settings,
            CollisionMap collisionMap,
            [Zenject.Inject(Id = "ExplosionEffect")] IFactory<MonoProduct> explosionEffectFactory)
        {
            _settings = settings;

            _collisionMap = collisionMap;

            _explosionEffectFactory = explosionEffectFactory;

            _pool = new CustomPool<PlayerMissile>(
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

        protected override MonoProduct Build(PlayerMissile product)
        {
            var basicMoveComponent = new BasicMoveComponent(product.transform, _settings);
            var missileMoveComponent = new MissileMoveComponent(product.transform, _settings);

            var timer = new Timer(product);

            var missileHomingStrategy = new PlayerMissileHomingStrategy(product.transform, missileMoveComponent, missileMoveComponent, _collisionMap);

            var missileLaunchState = new MissileLaunchState(product.transform, _settings, basicMoveComponent, timer);

            var missileHomingState = new MissileHomingState(product.transform, _settings, missileHomingStrategy, timer);
            missileHomingStrategy.OnHoming += missileHomingState.OnHoming;

            var missileIdleState = new MissileIdleState(product.transform, _settings, basicMoveComponent);

            var missileStateMachine = new StateMachine();

            missileStateMachine.AddTransition(missileLaunchState, missileHomingState,
                new FuncPredicate(() => missileLaunchState.IsLaunched));
            missileStateMachine.AddTransition(missileHomingState, missileIdleState,
                new FuncPredicate(() => !missileHomingState.IsHoming));
            missileStateMachine.AddState(missileIdleState);

            missileStateMachine.SetState(missileLaunchState);

            var stateMachineReset = new StateMachineReset(missileStateMachine, missileLaunchState);

            product.Construct(_settings, missileStateMachine, _explosionEffectFactory, stateMachineReset, _collisionMap);

            return product;
        }
    }
}
