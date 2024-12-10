using Ui.DialogWindows;
using Ui.DialogWindows.Dialogs;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private EventManager _events;

    public void Initialise()
    {
        _events = EntryPoint.Instance.Events;

        _events.PlayerDied += OnPlayerDied;
        _events.LevelCompleted += OnLevelCompleted;
        _events.MainMenuExit += OnMenuExit;
    }

    public void StartGame()
    {
        _events.Start?.Invoke();
    }

    public void StopGame()
    {
        _events.Stop?.Invoke();
    }

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
        gameOverDialog.Initialise(EntryPoint.Instance.ScoreController.Score);
    }

    private void OnLevelCompleted()
    {
        StopGame();
        YouWinDialog youWinDialog = DialogManager.ShowDialog<YouWinDialog>();
        youWinDialog.Initialise(EntryPoint.Instance.ScoreController.Score);
    }

    private void OnMenuExit()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;

        StopGame();
        SceneLoader.LoadScene(GlobalConstants.MainMenuSceneName);
    }

    private void OnDestroy()
    {
        _events.PlayerDied -= OnPlayerDied;
        _events.LevelCompleted -= OnLevelCompleted;
        _events.MainMenuExit -= OnMenuExit;
    }
}