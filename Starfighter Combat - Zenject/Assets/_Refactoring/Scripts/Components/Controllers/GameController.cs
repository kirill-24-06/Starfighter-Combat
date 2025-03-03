using System;
using UnityEngine;
using Utils.Events.Channel.Static;
using Refactoring.Ui.DialogWindows;
using Refactoring.Ui.DialogWindows.Dialogs;
using System.Threading;


namespace Refactoring
{
    public class GameController : IAwakeable, IDisposable
    {
        private ScoreController _scoreController;

        private CancellationTokenSource _cancellationTokenSource;

        public GameController(ScoreController scoreController, CancellationTokenSource sceneTokenSource)
        {
            _scoreController = scoreController;

            _cancellationTokenSource = sceneTokenSource;
        }

        public void Awake()
        {
            Channel<PlayerDiedEvent>.OnEvent += OnPlayerDied;
            Channel<LevelCompletedEvent>.OnEvent += OnLevelCompleted;
            Channel<MainMenuExitEvent>.OnEvent += OnMenuExit;
            Channel<PauseEvent>.OnEvent += OnGamePause;
        }

        public void StopGame() => Channel<StopEvent>.Raise(new StopEvent());

        private void OnGamePause(PauseEvent @event)
        {
            if (@event.Value)
            {
                Time.timeScale = 0;
                DialogManager.ShowDialog<PauseMenuDialog>();
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        private void OnPlayerDied(PlayerDiedEvent @event)
        {
            StopGame();
            var gameOverDialog = DialogManager.ShowDialog<GameOverWindow>();
            gameOverDialog.Initialise(_scoreController.Score);
        }

        private void OnLevelCompleted(LevelCompletedEvent @event)
        {
            StopGame();
            YouWinDialog youWinDialog = DialogManager.ShowDialog<YouWinDialog>();
            youWinDialog.Initialise(_scoreController.Score);
        }

        private void OnMenuExit(MainMenuExitEvent @event)
        {
            if (Time.timeScale == 0)
                Time.timeScale = 1;

            StopGame();
            SceneLoader.LoadScene(GlobalConstants.MainMenuSceneName);
        }

        public void Dispose()
        {
            Channel<PlayerDiedEvent>.OnEvent -= OnPlayerDied;
            Channel<LevelCompletedEvent>.OnEvent -= OnLevelCompleted;
            Channel<MainMenuExitEvent>.OnEvent -= OnMenuExit;
            Channel<PauseEvent>.OnEvent -= OnGamePause;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}

