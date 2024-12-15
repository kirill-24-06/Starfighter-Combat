using System;

public class ScoreController : IDisposable
{
    private EventManager _events;

    private int _score;
    public int Score => _score;

    public ScoreController(EventManager events)
    {
        _events = events;

        _events.Start += OnGameStarted;
        _events.AddScore += OnScoreAdded;
    }

    private void OnGameStarted()
    {
        _score = 0;
        _events.ChangeScore(Score);
    }

    private void OnScoreAdded(int value)
    {
        _score += value;
        _events.ChangeScore(_score);
    }

    public void Dispose()
    {
        _events.Start -= OnGameStarted;
        _events.AddScore -= OnScoreAdded;
    }
}