using System.Collections;
using UnityEngine;
using Utils.Extensions;

namespace Refactoring
{
    public class PatrolState : EnemyCombatState
    {
        private IPatrolData _data;
        private ILifetimeSettings _lifetimeSettings;

        private MonoBehaviour _context;
        private GameObject _contextGO;
        private RepositionState _patrolState;

        private Timer _liveTimer;

        private IEnumerator _await;
        private bool _awaiting = false;

        public bool LiveTimeIsOver { get; private set; }

        public PatrolState(MonoBehaviour client, IPatrolData data, ILifetimeSettings lifetimeSettings, Timer liveTimer, RepositionState patrolState) : base(client.transform)
        {
            _data = data;

            _context = client;
            _contextGO = _context.gameObject;

            _patrolState = patrolState;

            _lifetimeSettings = lifetimeSettings;

            _liveTimer = liveTimer;
            _liveTimer.TimeIsOver += () => LiveTimeIsOver = true;

            _await = Await();
        }

        public override void Enter()
        {
            _patrolState.Enter();
            _context.StartCoroutine(_await);

            _liveTimer.SetTimer(_lifetimeSettings.LifeTime);
            _liveTimer.StartTimer();
        }

        public override void Update()
        {
            if (_awaiting) return;

            _patrolState.Update();
        }

        //public override void Update()
        //{
        //    if (_patrolState.IsArrived && !_awaiting)
        //    {
        //        _context.StartCoroutine(_await);
        //        _awaiting = true;
        //    }

        //    else
        //    {
        //        _patrolState.Update();
        //    }
        //}

        public override void Exit() => _context.StopCoroutine(_await);

        //private IEnumerator Await()
        //{
        //    yield return WaitFor.Seconds(_data.AwaitTime);

        //    _patrolState.Enter();

        //    _awaiting = false;
        //}

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