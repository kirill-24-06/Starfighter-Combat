using UnityEngine;
using Utils.Events.Channel.Static;
using Utils.SpawnSystem;
using Utils.StateMachine;

namespace Refactoring
{
    public class Bonus : MonoProduct
    {
        private BonusSettings _data;

        private StateMachine _bonusStateMachine;
        private IResetable _stateMachine;

        private BonusTakenEvent _bonusTaken;
        private BonusCollectedEvent _bonusCollected;
        private AddScoreEvent _addScore;

        private bool _inPool = true;

        public void Construct(BonusSettings settings, StateMachine bonusStateMachine, IResetable stateMachineReset)
        {
            _data = settings;
            _bonusStateMachine = bonusStateMachine;
            _stateMachine = stateMachineReset;

            _bonusTaken = new BonusTakenEvent();
            _bonusCollected = new BonusCollectedEvent().SetTag(_data.BonusTag);
            _addScore = new AddScoreEvent(_data.Score);

            _inPool = false;

            IsConstructed = true;
        }

        #region MonoProduct
        public override MonoProduct OnGet()
        {
            if (!IsConstructed) return this;
             
           _stateMachine.Reset();
           
            _inPool = false;

            return this;
        }

        public override MonoProduct OnRelease()
        {
            Channel<BonusTakenEvent>.Raise(_bonusTaken);

            return this;
        }

        #endregion

        #region MonoBehaviour

        private void FixedUpdate() => _bonusStateMachine.OnFixedUpdate();
        private void Update() => _bonusStateMachine.OnUpdate();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (Player.IsPlayer(collision.gameObject))
            {
                Collide();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_inPool) return;
            _inPool = true;

            Release();
        }

        #endregion

        private void Collide()
        {
            if (_inPool) return;
            _inPool = true;

            Channel<BonusCollectedEvent>.Raise(_bonusCollected);
            Channel<AddScoreEvent>.Raise(_addScore);

            Release();
        }

    }
}