using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Utils.Pool.Generic;
using Utils.SpawnSystem;
using Utils.StateMachine;

namespace Refactoring
{
    public class BossDefenceDroneFactory : GenericFactory<BossDefenceDrone, BossDefenceDroneSettings>
    {
        private IFactory<MonoProduct> _effectsFactory;
        private IFactory<MonoProduct> _projectileFactory;

        private Bounds[] _patrolAreas;

        private Player _player;
        private CollisionMap _collisionMap;

        private CancellationToken _cancellationToken;

        public BossDefenceDroneFactory(
            BossDefenceDroneSettings settings,
            IFactory<MonoProduct> effectsFactory,
            [Zenject.Inject(Id = "BossMissile")] IFactory<MonoProduct> projectileFactory,
            Player player,
            CollisionMap collisionMap,
            Bounds[] patrolAreas,
            CancellationToken cancellationToken)
        {
            _settings = settings;
            _effectsFactory = effectsFactory;
            _projectileFactory = projectileFactory;
            _player = player;
            _collisionMap = collisionMap;
            _patrolAreas = patrolAreas;
            _cancellationToken = cancellationToken;

            _pool = new CustomPool<BossDefenceDrone>(
                _settings.Create,
                _settings.OnGet,
                _settings.OnRelease,
                _settings.PrewarmAmount);
        }

        public override MonoProduct Create()
        {
            var drone = _pool.Get();

            return !drone.IsConstructed ? Build(drone).WithRelease(Release) : drone;
        }

        protected override MonoProduct Build(BossDefenceDrone drone)
        {
            var basicMoveHandler = new BasicMoveComponent(drone.transform, _settings);
            var combatMoveHandler = new PointMove(drone.transform, _settings);
            var health = new EnemyHealthHandler(_settings.Health);
            var weapon = new EnemyCannon(drone, _projectileFactory, _settings);

            var resetables = new List<IResetable>()
            {
                health,
                weapon
            };

            var droneStateMachine = GetStateMachine(drone, basicMoveHandler, combatMoveHandler, weapon, resetables);

            drone.Construct(_settings, droneStateMachine, health, resetables,
                _effectsFactory, _player, _collisionMap, _cancellationToken);

            return drone;
        }

        private StateMachine GetStateMachine(BossDefenceDrone drone, BasicMoveComponent basicMoveHandler, PointMove combatMoveHandler, EnemyCannon weapon, List<IResetable> resetables)
        {
            var patrol = new TwoPointsReposition(drone.transform, _patrolAreas, combatMoveHandler);
            resetables.Add(patrol);

            var patrolState = new RepositionState(drone.transform, combatMoveHandler, patrol, false);

            var moveState = new MoveState(drone.transform, basicMoveHandler, _settings);
            var permanentPatrolState = new AttackWhileRepositionState(drone, _settings, patrolState, weapon, _player.transform);

            var droneStateMachine = new StateMachine();

            droneStateMachine.AddTransition(moveState, permanentPatrolState,
                new FuncPredicate(() => moveState.IsEnteredGameArea));
            droneStateMachine.AddState(permanentPatrolState);

            droneStateMachine.SetState(moveState);

            var stateMachineResetter = new StateMachineReset(droneStateMachine, moveState);
            resetables.Add(stateMachineResetter);

            return droneStateMachine;
        }
    }
}

