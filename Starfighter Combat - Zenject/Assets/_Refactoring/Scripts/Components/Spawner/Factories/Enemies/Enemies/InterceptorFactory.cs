using System.Threading;
using Utils.SpawnSystem;
using Utils.StateMachine;
using Utils.Pool.Generic;
using System.Collections.Generic;

namespace Refactoring
{
    public class InterceptorFactory : GenericFactory<Interceptor, InterceptorSettings>
    {
        private IFactory<MonoProduct> _effectsFactory;
        private IFactory<MonoProduct> _projectileFactory;

        private Player _player;

        private CollisionMap _collisionMap;

        private CancellationToken _cancellationToken;

        public InterceptorFactory(
            InterceptorSettings interceptorSettings,
            IFactory<MonoProduct> visualEffectsFactory,
            [Zenject.Inject(Id = "EnemyLaser")] IFactory<MonoProduct> projectileFactory,
            Player player,
            CollisionMap collisionMap,
            CancellationToken cancellationToken)
        {
            _settings = interceptorSettings;
            _effectsFactory = visualEffectsFactory;
            _projectileFactory = projectileFactory;
            _player = player;
            _collisionMap = collisionMap;

            _pool = new CustomPool<Interceptor>(
                _settings.Create,
                _settings.OnGet,
                _settings.OnRelease,
                _settings.PrewarmAmount
                );

            _cancellationToken = cancellationToken;
        }

        #region GenericFactory

        public override MonoProduct Create()
        {
            var product = _pool.Get();

            return !product.IsConstructed ? Build(product).WithRelease(Release) : product;
        }

        protected override MonoProduct Build(Interceptor interceptor)
        {
            var weapon = new EnemyTripleCannon(interceptor, _projectileFactory, _settings);

            var basicMoveHandler = new BasicMoveComponent(interceptor.transform, _settings);
            var combatMoveHandler = new PointMove(interceptor.transform, _settings);

            var health = new EnemyHealthComponent(_settings.Health);

            var timer = new Timer(interceptor);

            var resetables = new List<IResetable>()
            {
                weapon,
                health,
            };

            var interceptorStateMachine = GetStateMachine(interceptor, weapon, basicMoveHandler, combatMoveHandler, timer, resetables);

            interceptor.Construct(_settings, interceptorStateMachine, health,
                timer, resetables, _effectsFactory, _player, _collisionMap, _cancellationToken);

            return interceptor;
        }
        #endregion

        #region StateMachine

        private StateMachine GetStateMachine(Interceptor interceptor, EnemyTripleCannon weapon, IMoveComponent basicMoveHandler, PointMove combatMoveHandler, Timer timer, List<IResetable> resetables)
        {
            CombatState combatState = GetCombatState(interceptor, weapon, combatMoveHandler, timer);
            var moveState = new MoveState(interceptor.transform, basicMoveHandler, _settings);
            var retreatState = new RetreatState(interceptor.transform, basicMoveHandler);

            var interceptorStateMachine = new StateMachine();

            interceptorStateMachine.AddTransition(moveState, combatState,
                new FuncPredicate(() => moveState.IsEnteredGameArea));
            interceptorStateMachine.AddTransition(combatState, retreatState,
                new FuncPredicate(() => combatState.LiveTimeIsOver));
            interceptorStateMachine.AddState(retreatState);

            interceptorStateMachine.SetState(moveState);

            var stateMachineReset = new StateMachineReset(interceptorStateMachine, moveState);

            resetables.Add(stateMachineReset);

            return interceptorStateMachine;
        }

        private CombatState GetCombatState(Interceptor interceptor, EnemyTripleCannon weapon, PointMove combatMoveHandler, Timer timer)
        {
            var repositionStrategy = new RandomPointReposition(_settings, combatMoveHandler);

            var repositionState = new RepositionState(interceptor.transform, combatMoveHandler, repositionStrategy);

            var attackState = new AttackState(interceptor.transform, weapon, _player.transform);
            weapon.AttackRunComplete += attackState.OnAttackComplete;

            var combatStateMachine = new StateMachine();

            combatStateMachine.AddTransition(repositionState, attackState,
                new FuncPredicate(() => repositionState.IsArrived));
            combatStateMachine.AddTransition(attackState, repositionState,
                new FuncPredicate(() => attackState.AttackComplete));

            var combatState = new CombatState(combatStateMachine, repositionState, timer, _settings);

            return combatState;
        }

        #endregion
    }
}
