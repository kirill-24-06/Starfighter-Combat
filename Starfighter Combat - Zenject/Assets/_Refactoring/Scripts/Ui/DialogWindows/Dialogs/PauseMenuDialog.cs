using UnityEngine;
using UnityEngine.UI;
using Utils.Events.Channel.Static;

namespace Refactoring.Ui.DialogWindows.Dialogs
{
    public class PauseMenuDialog : Dialog
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _mainMenuButton;

        private void Start()
        {
            _continueButton.onClick.AddListener(Continue);
            _settingsButton.onClick.AddListener(GoToSettings);
            _mainMenuButton.onClick.AddListener(GoToMainMenu);
        }

        private void Continue()
        {
            Hide();
            Channel<PauseEvent>.Raise(new PauseEvent(false));
        }

        private void GoToSettings()
        {
            Hide();
            DialogManager.ShowDialog<SettingsDialog>();
        }

        private void GoToMainMenu()
        {
            Channel<MainMenuExitEvent>.Raise(new MainMenuExitEvent());
        }
    }

}

