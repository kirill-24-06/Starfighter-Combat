using System.Collections.Generic;
using UnityEngine;
using Utils.SpawnSystem;
using Utils.StateMachine;

namespace Refactoring
{
    public class InterceptorWithAbilitiesStageBuilder : IBossStageBuilder
    {
        private InterceptorWithAbilitiesStageSettings _settings;

        private IFactory<MonoProduct> _projectileFactory;

        private BossAbilitiesBuilder _bossAbilitiesBuilder;

        private Player _player;

        public InterceptorWithAbilitiesStageBuilder(
            InterceptorWithAbilitiesStageSettings settings,
            [Zenject.Inject(Id ="EnemyLaser")]IFactory<MonoProduct> projectileFactory,
            BossAbilitiesBuilder builder,
            Player player)
        {
            _settings = settings;

            _projectileFactory = projectileFactory;

            _bossAbilitiesBuilder = builder;

            _player = player;
        }

        public IBossStage Build(MonoBehaviour client, IReadOnlyProperty<int> currentHealth, List<IResetable> resetables)
        {
            var clientTransform = client.transform;

            var moveComponent = new PointMove(clientTransform, _settings);
            var repositionStrategy = new RandomPointReposition(_settings, moveComponent);

            var weapon = new EnemyTripleCannon(client, _projectileFactory, _settings);

            _bossAbilitiesBuilder.Initialize(_settings.AbilitiesSettings, clientTransform);
            var bossAbilities = GetAbilitiesComponent();

            var repositionState = new RepositionState(clientTransform, moveComponent, repositionStrategy);
            var attackState = new AttackState(clientTransform, weapon, _player.transform);
            weapon.AttackRunComplete += attackState.OnAttackComplete;

            var stateMachine = new StateMachine();

            stateMachine.AddTransition(repositionState, attackState,
                new FuncPredicate(() => repositionState.IsArrived));
            stateMachine.AddTransition(attackState, repositionState,
                new FuncPredicate(() => attackState.AttackComplete));

            var completeCondition = new HealthPercentCondition(currentHealth, _settings.HealthData.Value.Health, _settings.StageSwitchHealthPercent);

            var stage = new InterceptorWithAbilitiesStage(client, _settings, bossAbilities, completeCondition, stateMachine, repositionState);

            return stage;
        }

        private BossAbilitiesComponent GetAbilitiesComponent()
        {
            var abilities = new List<IBossAbility>();

            foreach (var ability in _settings.BossAbilities)
            {
                abilities.Add(_bossAbilitiesBuilder.Build(ability));
            }

            var bossAbilities = new BossAbilitiesComponent(abilities);

            return bossAbilities;
        }
    }
}