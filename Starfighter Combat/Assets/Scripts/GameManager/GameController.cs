using UnityEngine;

public class GameController : MonoBehaviour
{
    private EventManager _events;
    private UiManager _uiManager;

    public void Initialise()
    {
        _events = EventManager.GetInstance();
        _uiManager = EntryPoint.Instance.UiManager;

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

    public void PauseGame()
    {
        _events.Pause?.Invoke();
        //Show Pause Screen
    }

    private void OnPlayerDied()
    {
        StopGame();
        _uiManager.GameOverDialog();
    }

    private void OnLevelCompleted()
    {
        StopGame();
        //Show Victory Screen
    }

    private void OnMenuExit()
    {
        //OpenMenuScene
    }

    private void OnDestroy()
    {
        _events.PlayerDied -= OnPlayerDied;
        _events.LevelCompleted -= OnLevelCompleted;
        _events.MainMenuExit -= OnMenuExit;
    }
}