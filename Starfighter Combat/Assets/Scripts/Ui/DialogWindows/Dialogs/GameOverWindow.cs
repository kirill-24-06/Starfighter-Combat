using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


namespace Ui.DialogWindows.Dialogs
{
    public class GameOverWindow : Dialog
    {
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _mainMenuButton;

        [SerializeField] private TextMeshProUGUI _scoreText;

        //private EventManager _events;

        private void Start()
        {
            _retryButton.onClick.AddListener(TryAgain);
            _mainMenuButton.onClick.AddListener(GoToMenu);

            //_events = EventManager.GetInstance();
        }

        public void Initialise(int score)
        {
            _scoreText.text = "Score: " + score;
        }

        private void TryAgain()
        {
            SceneManager.LoadScene(0);
            //_events.Invoke(new RestartLevelSignal());
            //Hide();
        }

        private void GoToMenu()
        {

            SceneManager.LoadScene(1);
            //SceneManager.LoadScene(StringConstants.MENU_SCENE_NAME);
        }
    }
}
