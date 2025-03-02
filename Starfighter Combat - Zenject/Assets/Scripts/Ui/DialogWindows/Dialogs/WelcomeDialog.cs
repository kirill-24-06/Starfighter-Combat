using Ui.DialogWindows;
using UnityEngine;
using UnityEngine.UI;

namespace Legacy
{
    public class WelcomeDialog : Dialog
    {
        [SerializeField] private Button _startGameButton;
        private GameController _controller;

        private void Start()
        {
            _controller = EntryPoint.Instance.GameController;
            _startGameButton.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            _controller.StartGame();
            Hide();
        }
    }
}
