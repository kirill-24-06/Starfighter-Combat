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

    public void Initialise()
    {
        EventManager.GetInstance().ChangeScore += UpdateScore;
        EventManager.GetInstance().ChangeHealth += UpdateHealth;
        EventManager.GetInstance().BonusAmountUpdate += UpdateBonuses;
    }



    private void OnDisable()
    {
        EventManager.GetInstance().ChangeScore -= UpdateScore;
        EventManager.GetInstance().PlayerDamaged -= UpdateHealth;
        EventManager.GetInstance().PlayerHealed -= UpdateHealth;
        EventManager.GetInstance().BonusAmountUpdate -= UpdateBonuses;
    }

    private void UpdateScore(int newScore)
    {
        _scoreText.text = "Score: " + newScore;
    }

    private void UpdateHealth(int newHealth)
    {
        _livesText.text = "Lives: " + newHealth;
    }

    private void UpdateBonuses(int bonusesAmount)
    {
        _bonuseesText.text = "Bombs: " + bonusesAmount;
    }

    public void GameOverDialog()
    {
        _gameOverText.gameObject.SetActive(true);
    }
}
