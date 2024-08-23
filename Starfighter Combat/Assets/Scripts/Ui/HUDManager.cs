using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _maxScoreText;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private TimeBar _bonusTimer;
    [SerializeField] private GameObject _pauseMenu;

    public void Initialise()
    {
        EventManager.GetInstance().ChangeScore += UpdateScore;
        EventManager.GetInstance().BonusCollected += ActivateBonusTimer;
        EventManager.GetInstance().Pause += OnPause;

        _pauseButton.onClick.AddListener(Pause);

        _bonusTimer.Initialise(EntryPoint.Instance.Player.BonusTimer);
    }



    private void OnDestroy()
    {
        EventManager.GetInstance().ChangeScore -= UpdateScore;
        EventManager.GetInstance().BonusCollected -= ActivateBonusTimer;
        EventManager.GetInstance().Pause -= OnPause;

        _pauseButton.onClick.RemoveAllListeners();
    }

    private void UpdateScore(int newScore)
    {
        _scoreText.text = "Score: " + newScore;
    }

    private void ActivateBonusTimer(BonusTag tag)
    {
        if (tag == BonusTag.Multilaser || tag == BonusTag.ForceField)
            _bonusTimer.gameObject.SetActive(true);
    }

    private void OnPause(bool value)
    {
        _pauseMenu.SetActive(value);
    }

    public void GameOverDialog()
    {
        _gameOverText.gameObject.SetActive(true);
    }

    private void Pause()
    {
        EntryPoint.Instance.GameController.PauseGame(!_pauseMenu.activeInHierarchy);
    }
}
