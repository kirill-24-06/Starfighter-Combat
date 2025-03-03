using System;
using UnityEngine;


namespace Refactoring
{
    public class LevelStage
    {
        private readonly StageData _data;

        private readonly Timer _stageTimer;
        public bool IsStageCompleted { get; private set; }

        public Action StageStart;

        public LevelStage(Timer timer, StageData data)
        {
            _data = data;
            IsStageCompleted = false;

            _stageTimer = timer;
            _stageTimer.SetTimer(data.StageTime);
            _stageTimer.TimeIsOver += () => { IsStageCompleted = true; };
        }

        public void StartStage()
        {
            StageStart?.Invoke();
            _stageTimer.StartTimer();
        }

        public StageData GetData() => _data;
    }
}
