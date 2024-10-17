using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Ui.DialogWindows.Dialogs
{
    public class YouWinDialog : Dialog
    {
        [SerializeField] private Button _mainMenuButton;

        [SerializeField] private TextMeshProUGUI _scoreText;

        private void Start()
        {
            _mainMenuButton.onClick.AddListener(GoToMenu);
        }

        public void Initialise(int score)
        {
            _scoreText.text = "Score: " + score;
        }

        private void GoToMenu()
        {
            SceneManager.LoadScene(GlobalConstants.MainMenuSceneName);
        }
    }
}