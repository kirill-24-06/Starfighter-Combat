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

    private int _bonusAmount;
    private int _lives;

    public void Initialise()
    {
        EventManager.GetInstance().ChangeScore += UpdateScore;
        EventManager.GetInstance().PlayerDamaged += UpdateHealth;
        EventManager.GetInstance().PlayerHealed += UpdateHealth;
        EventManager.GetInstance().BonusAmountUpdate += UpdateBonuses;
        EventManager.GetInstance().Start += OnStart;
    }

    private void OnStart()
    {
        UpdateBonuses(0);
        UpdateHealth(EntryPoint.Instance.Player.ObjectInfo.Health);
    }

    private void OnDisable()
    {
        EventManager.GetInstance().ChangeScore -= UpdateScore;
        EventManager.GetInstance().PlayerDamaged -= UpdateHealth;
        EventManager.GetInstance().PlayerHealed -= UpdateHealth;
        EventManager.GetInstance().BonusAmountUpdate -= UpdateBonuses;
        EventManager.GetInstance().Start -= OnStart;
    }

    private void UpdateScore(int newScore)
    {
        _scoreText.text = "Score: " + newScore;
    }

    private void UpdateHealth(int newHealth)
    {
        _lives = newHealth;
        _livesText.text = "Lives: " + _lives;
    }

    private void UpdateBonuses(int bonusesToAdd)
    {
        _bonusAmount += bonusesToAdd;
        _bonuseesText.text = "Bombs: " + _bonusAmount;
    }

    public void GameOverDialog()
    {
        _gameOverText.gameObject.SetActive(true);
        //Time.timeScale = 0;
    }
}
