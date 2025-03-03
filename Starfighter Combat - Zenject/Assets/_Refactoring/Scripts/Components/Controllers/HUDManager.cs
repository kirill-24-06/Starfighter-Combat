using System;
using TMPro;
using UnityEngine.UI;
using Utils.Events.Channel.Static;

namespace Refactoring
{
    public class HUDManager : IAwakeable, IDisposable
    {
        private TextMeshProUGUI _scoreText;
        private Button _pauseButton;
        private TimeBar _bonusTimer;

        private HealthBar _healthBar;
        private BombsBar _bombsBar;
        private BossHealthBar _bossHealthBar;

        public HUDManager(
            TextMeshProUGUI scoreText,
            Button pauseButton,
            TimeBar bonusTimer,
            BossHealthBar bossHealthBar,
            HealthBar healthBar,
            BombsBar bombsBar)
        {
            _scoreText = scoreText;

            _pauseButton = pauseButton;

            _bonusTimer = bonusTimer;

            _bossHealthBar = bossHealthBar;

            _healthBar = healthBar;

            _bombsBar = bombsBar;
        }

        public void Awake()
        {
            _pauseButton.onClick.AddListener(Pause);

            Channel<ChangeScoreEvent>.OnEvent += UpdateScore;
            Channel<PauseEvent>.OnEvent += OnPause;

            Channel<BossArrivalEvent>.OnEvent += _bossHealthBar.OnBossArrival;
            Channel<BossDamagedEvent>.OnEvent += _bossHealthBar.UpdateHealthPrecent;
            Channel<BossDefeatEvent>.OnEvent += _bossHealthBar.OnBossDefeat;

            Channel<StopEvent>.OnEvent += OnStop;
            Channel<StartEvent>.OnEvent += OnStart;

            Channel<ChangeHealthEvent>.OnEvent += _healthBar.Show;

            Channel<BombsAmountUpdateEvent>.OnEvent += _bombsBar.Show;
        }

        private void UpdateScore(ChangeScoreEvent @event) => _scoreText.text = @event.Score.ToString();

        public void ActivateBonusTimer() => _bonusTimer.gameObject.SetActive(true);

        private void Pause() => Channel<PauseEvent>.Raise(new PauseEvent(true));

        private void OnPause(PauseEvent @event) => _pauseButton.gameObject.SetActive(!@event.Value);

        private void OnStart(StartEvent @event) => _pauseButton.gameObject.SetActive(true);

        private void OnStop(StopEvent @event)
        {
            _pauseButton.gameObject.SetActive(false);
            _bossHealthBar.OnGameStop();
        }

        public void Dispose()
        {
            Channel<ChangeScoreEvent>.OnEvent -= UpdateScore;
            Channel<PauseEvent>.OnEvent -= OnPause;

            Channel<BossArrivalEvent>.OnEvent -= _bossHealthBar.OnBossArrival;
            Channel<BossDamagedEvent>.OnEvent -= _bossHealthBar.UpdateHealthPrecent;
            Channel<BossDefeatEvent>.OnEvent -= _bossHealthBar.OnBossDefeat;

            Channel<StopEvent>.OnEvent -= OnStop;
            Channel<StartEvent>.OnEvent -= OnStart;

            Channel<ChangeHealthEvent>.OnEvent -= _healthBar.Show;

            Channel<BombsAmountUpdateEvent>.OnEvent -= _bombsBar.Show;

            _pauseButton.onClick.RemoveAllListeners();
        }

    }
}