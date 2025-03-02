using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ui.DialogWindows;

namespace Legacy
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
            SceneLoader.LoadScene(GlobalConstants.MainMenuSceneName);
        }
    }
}