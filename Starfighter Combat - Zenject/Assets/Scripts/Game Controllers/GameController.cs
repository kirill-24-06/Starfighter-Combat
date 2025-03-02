using System;
using Ui.DialogWindows;
using Legacy;
using UnityEngine;

public class GameController : IDisposable
{
    private EventManager _events;
    private ScoreController _scoreController;

    public GameController(ScoreController scoreController,EventManager events)
    {
        _scoreController = scoreController;
        _events = events;

        _events.PlayerDied += OnPlayerDied;
        _events.LevelCompleted += OnLevelCompleted;
        _events.MainMenuExit += OnMenuExit;
    }

    public void StartGame() => _events.Start?.Invoke();

    public void StopGame() => _events.Stop?.Invoke();

    public void PauseGame(bool value)
    {
        _events.Pause?.Invoke(value);

        if (value)
        {
            Time.timeScale = 0;
            DialogManager.ShowDialog<PauseMenuDialog>();
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void OnPlayerDied()
    {
        StopGame();
        GameOverWindow gameOverDialog = DialogManager.ShowDialog<GameOverWindow>();
        gameOverDialog.Initialise(_scoreController.Score);
    }

    private void OnLevelCompleted()
    {
        StopGame();
        YouWinDialog youWinDialog = DialogManager.ShowDialog<YouWinDialog>();
        youWinDialog.Initialise(_scoreController.Score);
    }

    private void OnMenuExit()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;

        StopGame();
        SceneLoader.LoadScene(GlobalConstants.MainMenuSceneName);
    }

    public void Dispose()
    {
        _events.PlayerDied -= OnPlayerDied;
        _events.LevelCompleted -= OnLevelCompleted;
        _events.MainMenuExit -= OnMenuExit;
    }
}