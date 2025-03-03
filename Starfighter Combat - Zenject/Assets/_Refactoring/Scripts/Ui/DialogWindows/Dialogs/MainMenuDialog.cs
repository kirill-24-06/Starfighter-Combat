using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Refactoring.Ui.DialogWindows.Dialogs
{
    public class MainMenuDialog : Dialog
    {
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitGameButton;

        protected override void Awake()
        {
            base.Awake();

            _newGameButton.onClick.AddListener(StartNewGame);
            _settingsButton.onClick.AddListener(OpenSettings);
            _exitGameButton.onClick.AddListener(ExitGame);
        }

        private void StartNewGame()
        {
            SceneLoader.LoadScene(GlobalConstants.MainSceneName);
            Hide();
        }

        private void OpenSettings()
        {
            Hide();
            DialogManager.ShowDialog<SettingsDialog>();
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        }
    }

}

