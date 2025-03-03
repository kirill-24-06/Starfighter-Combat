using Utils.Pool.Generic;
using Utils.SpawnSystem;
using Utils.StateMachine;

namespace Refactoring
{
    public class BonusFactory : GenericFactory<Bonus, BonusSettings>
    {
        public BonusFactory(BonusSettings settings)
        {
            _settings = settings;

            _pool = new CustomPool<Bonus>(
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

        protected override MonoProduct Build(Bonus product)
        {
            var basicMoveComponent = new BasicMoveComponent(product.transform, _settings);
            var advancedMoveComponent = new PointMove(product.transform, _settings);

            var timer = new Timer(product);

            var repositionStrategy = new RandomPointReposition(_settings, advancedMoveComponent);
            var repositionState = new RepositionState(product.transform, advancedMoveComponent, repositionStrategy, false);

            var moveState = new MoveState(product.transform, basicMoveComponent, _settings);
            var patrolState = new PatrolState(product, _settings,_settings,timer, repositionState);
            var retreatState = new RetreatState(product.transform, basicMoveComponent);

            var bonusStateMachine = new StateMachine();

            bonusStateMachine.AddTransition(moveState, patrolState,
                new FuncPredicate(() => moveState.IsEnteredGameArea));
            bonusStateMachine.AddTransition(patrolState, retreatState,
                new FuncPredicate(() => patrolState.LiveTimeIsOver));
            bonusStateMachine.AddState(retreatState);

            bonusStateMachine.SetState(moveState);

            var stateMachineReset = new StateMachineReset(bonusStateMachine, moveState);

            product.Construct(_settings, bonusStateMachine, stateMachineReset);

            return product;
        }
    }
}