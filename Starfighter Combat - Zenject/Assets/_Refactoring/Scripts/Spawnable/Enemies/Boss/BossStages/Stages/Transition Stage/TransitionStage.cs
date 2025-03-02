using UnityEngine;
using Utils.Events.Channel.Static;

namespace Refactoring
{
    public class TransitionStage : BossStage
    {
        private MinionSettings[] _minions;
        private GameObject _transitionStageShield;

        private int _activeMinionsCount;

        public override bool StageCompleted => _activeMinionsCount <= 0;

        public TransitionStage(
            MinionSettings[] minions,
            GameObject transitionStageShield)
        {
            _minions = minions;
            _transitionStageShield = transitionStageShield;
        }

        public override void Enter()
        {
            Channel<BossInvunrableEvent>.Raise(new BossInvunrableEvent(true));
            OnStageStart();

            Channel<EnemyDestroyedEvent>.OnEvent += OnEnemyDestroyed;
        }

        public override void Exit()
        {
            Channel<BossInvunrableEvent>.Raise(new BossInvunrableEvent(false));
            _transitionStageShield.SetActive(false);

            Channel<EnemyDestroyedEvent>.OnEvent -= OnEnemyDestroyed;
        }

        private void OnStageStart()
        {
            _transitionStageShield.SetActive(true);

            for (int i = 0; i < _minions.Length; i++)
            {
                for (int j = 0; j < _minions[i].AmountToSpawn; j++)
                {
                    var minion = _minions[i].Minion.Value;

                    Channel<SpawnMinionEvent>.Raise(new SpawnMinionEvent(minion));

                    _activeMinionsCount++;
                }
            }
        }

        private void OnEnemyDestroyed(EnemyDestroyedEvent @event)
        {
            _activeMinionsCount--;

            if (_activeMinionsCount < 0)
            {
                _activeMinionsCount = 0;
            }
        }
    }

    public struct SpawnMinionEvent : IEvent
    {
        public IEnemySpawnSettings Minion { get; private set; }

        public SpawnMinionEvent(IEnemySpawnSettings minion) => Minion = minion;
    }
}