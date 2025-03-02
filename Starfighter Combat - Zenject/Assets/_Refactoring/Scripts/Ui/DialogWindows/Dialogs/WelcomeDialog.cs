using UnityEngine;
using UnityEngine.UI;
using Utils.Events.Channel.Static;

namespace Refactoring.Ui.DialogWindows.Dialogs
{
    public class WelcomeDialog : Dialog
    {
        [SerializeField] private Button _startGameButton;

        private void Start()
        {
            _startGameButton.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            Channel<StartEvent>.Raise(new StartEvent());
            Hide();
        }
    }

}

