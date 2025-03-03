using System.Collections;
using UnityEngine;
using Utils.Extensions;

namespace Refactoring
{
    public class AttackWhileRepositionState : EnemyCombatState
    {
        private IPatrolData _data;

        private MonoBehaviour _context;
        private GameObject _contextGO;

        private RepositionState _patrolState;

        private Transform _target;

        private IWeapon _weapon;

        private IEnumerator _await;
        private bool _awaiting = false;

        public AttackWhileRepositionState(MonoBehaviour client, IPatrolData data,
            RepositionState patrolState, IWeapon weapon, Transform target) : base(client.transform)
        {
            _data = data;
            _context = client;
            _contextGO = _context.gameObject;
            _patrolState = patrolState;
            _weapon = weapon;
            _target = target;
            _await = Await();
        }

        #region IState
        public override void Enter()
        {
            _patrolState.Enter();
            _context.StartCoroutine(_await);
        }

        public override void FixedUpdate() => Attack();

        public override void Update()
        {
            if (_awaiting) return;

            _patrolState.Update();
        }

        public override void Exit() => _context.StopCoroutine(_await);

        #endregion

        private void Attack()
        {
            if (_target.gameObject.activeInHierarchy)
            {
                RotateTowardsTarget(_target.position);
                _weapon.Attack();
            }
        }

        private IEnumerator Await()
        {
            while (_contextGO.activeInHierarchy)
            {
                if (_patrolState.IsArrived)
                {
                    _awaiting = true;

                    yield return WaitFor.Seconds(_data.AwaitTime);

                    _patrolState.Enter();

                    _awaiting = false;
                }

                yield return null;
            }
        }
    }
}
