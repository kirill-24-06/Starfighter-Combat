using System;
using TMPro;
using UnityEngine.UI;

public class HUDManager : IDisposable
{
    private TextMeshProUGUI _scoreText;
    private Button _pauseButton;
    private TimeBar _bonusTimer;

    private HealthBar _healthBar;
    private BombsBar _bombsBar;
    private BossHealthBar _bossHealthBar;

    private GameController _gameController;
    private EventManager _events;

    public HUDManager(HUDElements hudElements,GameController gameController ,EventManager events)
    {
        _gameController = gameController;
        _events = events;
        _events.ChangeScore += UpdateScore;
        _events.Pause += OnPause;

        _scoreText = hudElements.ScoreText;

        _pauseButton = hudElements.PauseButton;
        _pauseButton.onClick.AddListener(Pause);

        _bonusTimer = hudElements.BonusTimer;

        _bossHealthBar = hudElements.BossHealthBar;
        _events.BossArrival += _bossHealthBar.OnBossArrival;
        _events.BossDamaged += _bossHealthBar.UpdateHealthPrecent;
        _events.BossDefeated += _bossHealthBar.OnBossDefeat;
        _events.Start += OnStart;
        _events.Stop += OnStop; 

        _healthBar = hudElements.HealthBar;
        _events.ChangeHealth += _healthBar.Show;

        _bombsBar = hudElements.BombsBar;
        _events.BonusAmountUpdate += _bombsBar.Show;
    }

    private void UpdateScore(int newScore) => _scoreText.text = newScore.ToString();

    public void ActivateBonusTimer() => _bonusTimer.gameObject.SetActive(true); 

    private void Pause() => _gameController.PauseGame(true);
   
    private void OnPause(bool value) => _pauseButton.gameObject.SetActive(!value);

    private void OnStart() => _pauseButton.gameObject.SetActive(true);

    private void OnStop()
    {
        _pauseButton.gameObject.SetActive(false);
        _bossHealthBar.OnGameStop();
    }

    public void Dispose()
    {
        _events.ChangeScore -= UpdateScore;
        _events.Pause -= OnPause;

        _events.BossArrival -= _bossHealthBar.OnBossArrival;
        _events.BossDamaged -= _bossHealthBar.UpdateHealthPrecent;
        _events.BossDefeated -= _bossHealthBar.OnBossDefeat;
        _events.Stop -= OnStop;
        _events.Start -= OnStart;

        _events.ChangeHealth -= _healthBar.Show;

        _events.BonusAmountUpdate -= _bombsBar.Show;

        _pauseButton.onClick.RemoveAllListeners();
    }
}