using System.Collections.Generic;
using UnityEngine;
using Utils.SpawnSystem;
using Utils.StateMachine;

namespace Refactoring
{
    public class InterceptorStageBuilder : IBossStageBuilder
    {
        private InterceptorStageSettings _settings;

        private IFactory<MonoProduct> _projectileFactory;

        private Player _player;

        public InterceptorStageBuilder(
            InterceptorStageSettings settings,
            [Zenject.Inject(Id = "EnemyLaser")] IFactory<MonoProduct> projectileFactory,
            Player player)
        {
            _settings = settings;

            _projectileFactory = projectileFactory;

            _player = player;
        }

        public IBossStage Build(MonoBehaviour client, IReadOnlyProperty<int> currentHealth, List<IResetable> resetables)
        {
            var productTransform = client.transform;

            var moveComponent = new PointMove(productTransform, _settings);
            var repositionStrategy = new RandomPointReposition(_settings, moveComponent);

            var weapon = new EnemyTripleCannon(client, _projectileFactory, _settings);
            resetables.Add(weapon);

            var repositionState = new RepositionState(productTransform, moveComponent, repositionStrategy);
            var attackState = new AttackState(productTransform, weapon, _player.transform);
            weapon.AttackRunComplete += attackState.OnAttackComplete;

            var stateMachine = new StateMachine();

            stateMachine.AddTransition(repositionState, attackState,
                new FuncPredicate(() => repositionState.IsArrived));
            stateMachine.AddTransition(attackState, repositionState,
                new FuncPredicate(() => attackState.AttackComplete));

            var stageCompleteCondition = new HealthPercentCondition(currentHealth, _settings.HealthData.Value.Health, _settings.StageSwitchHealthPercent);

            var stage = new InterceptorStage(stageCompleteCondition, stateMachine, repositionState);

            return stage;
        }
    }
}