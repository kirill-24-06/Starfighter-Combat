using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private EventManager _events;

    private int _score;
    public int Score => _score;

    public void Initialise()
    {
        _events = EntryPoint.Instance.Events;

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

    private void OnDestroy()
    {
        _events.Start -= OnGameStarted;
        _events.AddScore -= OnScoreAdded;
    }
}