using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Utils.Pool.Generic;
using Utils.SpawnSystem;
using Utils.StateMachine;

namespace Refactoring
{
    public class RocketShipFactory : GenericFactory<RocketShip, RocketShipSettings>
    {
        private IFactory<MonoProduct> _effectsFactory;
        private IFactory<MonoProduct> _projectileFactory;

        private Bounds[] _patrolAreas;

        private Player _player;

        private CollisionMap _collisionMap;

        private CancellationToken _token;

        public RocketShipFactory(
            RocketShipSettings settings,
            Bounds[] patrolArea,
            IFactory<MonoProduct> effectsFactory,
            [Zenject.Inject(Id = "EnemyMissile")] IFactory<MonoProduct> projectileFactory,
            Player player,
            CollisionMap collisionMap,
            CancellationToken token)
        {
            _settings = settings;
            _token = token;
            _patrolAreas = patrolArea;
            _effectsFactory = effectsFactory;
            _projectileFactory = projectileFactory;
            _player = player;
            _collisionMap = collisionMap;

            _pool = new CustomPool<RocketShip>(
                _settings.Create,
                _settings.OnGet,
                _settings.OnRelease,
                _settings.PrewarmAmount);
        }

        #region GenericFactory
        public override MonoProduct Create()
        {
            var product = _pool.Get();

            return !product.IsConstructed ? Build(product).WithRelease(Release) : product;
        }

        protected override MonoProduct Build(RocketShip rocketShip)
        {
            var basicMoveHandler = new BasicMoveComponent(rocketShip.transform, _settings);
            var combatMoveHandler = new PointMove(rocketShip.transform, _settings);

            var health = new EnemyHealthHandler(_settings.Health);

            var weapon = new EnemyTripleCannon(rocketShip, _projectileFactory, _settings);

            var timer = new Timer(rocketShip);

            var resetables = new List<IResetable>()
            {
                weapon,
                health,
            };

            var rocketShipStateMachine = GetStateMachine(rocketShip, basicMoveHandler, combatMoveHandler, weapon, timer, resetables);

            rocketShip.Construct(_settings, rocketShipStateMachine, health,
                timer, resetables, _effectsFactory, _player, _collisionMap, _token);

            return rocketShip;
        }

        #endregion

        #region StateMachine
        private StateMachine GetStateMachine(RocketShip rocketShip, BasicMoveComponent basicMoveHandler, PointMove combatMoveHandler, EnemyTripleCannon weapon, Timer timer, List<IResetable> resetables)
        {
            var moveState = new MoveState(rocketShip.transform, basicMoveHandler, _settings);
            var combatState = GetCombatState(rocketShip, combatMoveHandler, weapon, timer, resetables);
            var retreatState = new RetreatState(rocketShip.transform, basicMoveHandler);

            var rocketShipStateMachine = new StateMachine();

            rocketShipStateMachine.AddTransition(moveState, combatState,
                new FuncPredicate(() => moveState.IsEnteredGameArea));
            rocketShipStateMachine.AddTransition(combatState, retreatState,
                new FuncPredicate(() => combatState.LiveTimeIsOver));
            rocketShipStateMachine.AddState(retreatState);

            rocketShipStateMachine.SetState(moveState);

            var stateMachineResetter = new StateMachineReset(rocketShipStateMachine, moveState);
            resetables.Add(stateMachineResetter);

            return rocketShipStateMachine;
        }

        private CombatState GetCombatState(RocketShip rocketShip, PointMove combatMoveHandler, EnemyTripleCannon weapon, Timer timer, List<IResetable> resetables)
        {
            var patrol = new TwoPointsReposition(rocketShip.transform, _patrolAreas, combatMoveHandler);
            resetables.Add(patrol);

            var patrolState = new RepositionState(rocketShip.transform, combatMoveHandler, patrol);

            var attackState = new AttackState(rocketShip.transform, weapon, _player.transform);
            weapon.AttackRunComplete += attackState.OnAttackComplete;

            var combatStateMachine = new StateMachine();

            combatStateMachine.AddTransition(patrolState, attackState,
                new FuncPredicate(() => patrolState.IsArrived));
            combatStateMachine.AddTransition(attackState, patrolState,
                new FuncPredicate(() => attackState.AttackComplete));

            var combatState = new CombatState(combatStateMachine, patrolState, timer, _settings);

            return combatState;
        }

        #endregion
    }
}
