using System;
using Utils.Events.Channel.Static;

namespace Refactoring
{
    public class ScoreController :IAwakeable, IDisposable
    {
        private int _score;
        public int Score => _score;

        private ChangeScoreEvent _changeScore;

        public void Awake()
        {
            Channel<StartEvent>.OnEvent += OnGameStarted;
            Channel<AddScoreEvent>.OnEvent += OnScoreAdded;
        }

        private void OnGameStarted(StartEvent @event)
        {
            _score = 0;
            Channel<ChangeScoreEvent>.Raise(_changeScore.SetInt(Score));
        }

        private void OnScoreAdded(AddScoreEvent @event)
        {
            _score += @event.Score;
            Channel<ChangeScoreEvent>.Raise(_changeScore.SetInt(Score));
        }

        public void Dispose()
        {
            Channel<StartEvent>.OnEvent -= OnGameStarted;
            Channel<AddScoreEvent>.OnEvent -= OnScoreAdded;
        }
    }
}

