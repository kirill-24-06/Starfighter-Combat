using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ui.DialogWindows;


namespace Legacy
{
    public class GameOverWindow : Dialog
    {
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _mainMenuButton;

        [SerializeField] private TextMeshProUGUI _scoreText;
     
        private void Start()
        {
            _retryButton.onClick.AddListener(TryAgain);
            _mainMenuButton.onClick.AddListener(GoToMenu);
        }

        public void Initialise(int score)
        {
            _scoreText.text = "Score: " + score;
        }

        private void TryAgain()
        {
            SceneLoader.LoadScene(GlobalConstants.MainSceneName);
        }

        private void GoToMenu()
        {
            SceneLoader.LoadScene(GlobalConstants.MainMenuSceneName);
        }
    }
}
