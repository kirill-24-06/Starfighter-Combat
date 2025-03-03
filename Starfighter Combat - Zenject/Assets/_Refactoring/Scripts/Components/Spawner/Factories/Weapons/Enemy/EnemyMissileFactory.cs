using Utils.Pool.Generic;
using Utils.SpawnSystem;
using Utils.StateMachine;

namespace Refactoring
{
    public class EnemyMissileFactory : GenericFactory<EnemyMissile, EnemyMissileSettings>
    {
        private IFactory<MonoProduct> _explosionEffectFactory;
        private CollisionMap _collisionMap;

        private Player _player;

        public EnemyMissileFactory(
            EnemyMissileSettings settings,
            [Zenject.Inject(Id = "ExplosionEffect")] IFactory<MonoProduct> explosionEffectFactory,
            CollisionMap collisionMap,
            Player player)
        {
            _settings = settings;

            _explosionEffectFactory = explosionEffectFactory;

            _collisionMap = collisionMap;

            _player = player;

            _pool = new CustomPool<EnemyMissile>(
               _settings.Create,
               _settings.OnGet,
               _settings.OnRelease,
               _settings.PrewarmAmount);
        }

        public override MonoProduct Create()
        {
            var missile = _pool.Get();

            return !missile.IsConstructed ? Build(missile).WithRelease(Release) : missile;
        }

        protected override MonoProduct Build(EnemyMissile product)
        {
            var basicMoveComponent = new BasicMoveComponent(product.transform, _settings);
            var missileMoveComponent = new MissileMoveComponent(product.transform, _settings);

            var timer = new Timer(product);

            var homingStrategy = new EnemyMissileHomingStrategy(product.transform, missileMoveComponent, missileMoveComponent, _player.transform);

            var missileLaunchState = new MissileLaunchState(product.transform, _settings, basicMoveComponent, timer);

            var missileHomingState = new MissileHomingState(product.transform, _settings, homingStrategy, timer);
            homingStrategy.OnHoming += missileHomingState.OnHoming;

            var missileIdleState = new MissileIdleState(product.transform, _settings, basicMoveComponent);

            var missileStateMachine = new StateMachine();

            missileStateMachine.AddTransition(missileLaunchState, missileHomingState,
                new FuncPredicate(() => missileLaunchState.IsLaunched));
            missileStateMachine.AddTransition(missileHomingState, missileIdleState,
                new FuncPredicate(() => !missileHomingState.IsHoming));
            missileStateMachine.AddState(missileIdleState);

            missileStateMachine.SetState(missileLaunchState);

            var stateMachineReset = new StateMachineReset(missileStateMachine, missileLaunchState);

            product.Construct(_settings, missileStateMachine, _explosionEffectFactory, stateMachineReset, _player, _collisionMap);

            return product;
        }
    }
}
