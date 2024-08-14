using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject _inGameUi;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _maxScoreText;
    [SerializeField] private TextMeshProUGUI _livesText;
    [SerializeField] private TextMeshProUGUI _bonuseesText;
    [SerializeField] private TextMeshProUGUI _gameOverText;

    private int _score;
    private int _maxScore;
    private int _lives;

    public void Initialise()
    {
        EventManager.GetInstance().AddScore += UpdateScore;
        EventManager.GetInstance().PlayerDamaged += UpdateHealth;
        EventManager.GetInstance().PlayerHealed += UpdateHealth;
        EventManager.GetInstance().PlayerDied += OnPlayerDied;
        EventManager.GetInstance().Start += OnStart;
    }

    private void OnStart()
    {
        UpdateScore(0);
        UpdateHealth(EntryPoint.Player.ObjectInfo.Health);
    }

    private void UpdateScore(int scoreToAdd)
    {
        _score += scoreToAdd;
        _scoreText.text = "Score: " + _score;
    }

    private void UpdateHealth(int newHealth)
    {
        _lives = newHealth;
        _livesText.text = "Lives: " + _lives;
    }

    private void OnPlayerDied()
    {
        _gameOverText.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}
