using System.Collections.Generic;
using Utils.Pool.Generic;
using Utils.SpawnSystem;
using Utils.StateMachine;

namespace Refactoring
{
    public class EnemyAceFactory : GenericFactory<EnemyAce, AceSettings>
    {
        private Player _player;
        private CollisionMap _collisionMap;
        private IFactory<MonoProduct> _explosionEffectsFactory;
        private IBossStageBuilder[] _bossStages;

        public EnemyAceFactory(
            AceSettings settings,
            IFactory<MonoProduct> visualEffectsFactory,
            IBossStageBuilder[] bossStages,
            Player player,
            CollisionMap collisionMap)
        {
            _settings = settings;

            _collisionMap = collisionMap;

            _player = player;

            _explosionEffectsFactory = visualEffectsFactory;

            _bossStages = bossStages;

            _pool = new CustomPool<EnemyAce>(
                _settings.Create,
                _settings.OnGet,
                _settings.OnRelease,
                _settings.PrewarmAmount);

            var boss = Create();
            Release(boss);
        }

        public override MonoProduct Create()
        {
            var boss = _pool.Get();

            return !boss.IsConstructed ? Build(boss).WithRelease(Release) : boss;
        }

        protected override MonoProduct Build(EnemyAce product)
        {
            var productTransform = product.transform;

            var moveComponent = new BasicMoveComponent(productTransform, _settings);

            var healthComponent = new BossHealthComponent(_settings.Health);

            var resetables = new List<IResetable>()
            {
                healthComponent,
            };

            var combatState = GetCombatState(product, healthComponent, resetables);
            var moveState = new MoveState(productTransform, moveComponent, _settings);

            var bossStateMachine = new StateMachine();

            bossStateMachine.AddTransition(moveState, combatState,
                new FuncPredicate(() => moveState.IsEnteredGameArea));
            bossStateMachine.AddState(combatState);

            bossStateMachine.SetState(moveState);

            var stateMachineReset = new StateMachineReset(bossStateMachine, moveState);
            resetables.Add(stateMachineReset);

            product
                .Construct(
                 _settings,
                 bossStateMachine,
                 healthComponent,
                 resetables,
                 _explosionEffectsFactory,
                 _player,
                 _collisionMap);

            return product;
        }

        private BossCombatState GetCombatState(EnemyAce product, BossHealthComponent healthComponent, List<IResetable> resetables)
        {
            var stages = new IBossStage[_bossStages.Length];

            for (int i = 0; i < _bossStages.Length; i++)
            {
                stages[i] = _bossStages[i].Build(product, healthComponent, resetables);
            }

            var combatState = new BossCombatState(stages);

            return combatState;
        }
    }
}