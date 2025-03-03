using Utils.Events.Channel.Static;
using System;
using Zenject;


namespace Refactoring
{
    public class LevelController : IAwakeable, IDisposable, ITickable
    {
        private LevelData _data;

        private SpawnController _spawnController;

        private LevelStage[] _stages;
        private int currentStage = 0;

        private LevelCompletedEvent _levelCompleted;

        private bool _isGameActive;
        private bool _isBossFight = false;
        private bool _isBossDefeated = false;

        [Inject]
        public void Construct(LevelData data, LevelStage[] stages, SpawnController spawnController)
        {
            _data = data;
            _spawnController = spawnController;

            _stages = stages;

            for (int i = 0; i < _data.Stages.Count; i++)
                _stages[i].StageStart += OnNewStageStart;

            _levelCompleted = new LevelCompletedEvent();
        }

        public void Awake()
        {
            Channel<StartEvent>.OnEvent += OnStart;
            Channel<StopEvent>.OnEvent += OnStop;
            Channel<BossDefeatEvent>.OnEvent += OnBossDefeat;
        }

        private void OnStart(StartEvent @event)
        {
            _isGameActive = true;
            _stages[currentStage].StartStage();
        }

        private void OnStop(StopEvent @event) => _isGameActive = false;

        public void Tick() => Update();

        private void Update()
        {
            if (!_isGameActive)
                return;

            CheckLevelCompletion();
        }

        private void CheckLevelCompletion()
        {
            if (_isBossFight)
            {
                if (!_isBossDefeated)
                    return;

                Channel<LevelCompletedEvent>.Raise(_levelCompleted);
            }

            else if (_stages[currentStage].IsStageCompleted)
                AdvanceToNextStage();
        }

        private void OnBossDefeat(BossDefeatEvent @event) => _isBossDefeated = true;

        private void AdvanceToNextStage()
        {
            if (currentStage + 1 < _stages.Length)
            {
                currentStage++;
                _stages[currentStage].StartStage();
            }

            else
            {
                _isBossFight = true;
                _spawnController.BossArrival(_data.BossWave, _data.BossWaveDelay).Forget();
            }
        }

        private void OnNewStageStart()
        {
            _spawnController.NewStage(_stages[currentStage].GetData().SpawnerData);
        }

        public void Dispose()
        {
            Channel<StartEvent>.OnEvent -= OnStart;
            Channel<StopEvent>.OnEvent -= OnStop;
            Channel<BossDefeatEvent>.OnEvent -= OnBossDefeat;
        }
    }
}
