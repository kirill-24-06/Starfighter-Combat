using UnityEngine;
using UnityEngine.UI;

namespace Refactoring
{
    public class BossHealthBar
    {
        private GameObject _bossText;

        private GameObject _healthBarGameObject;
        private Slider _healthBar;

        public BossHealthBar(GameObject bossText, Slider bossHealthBar)
        {
            _bossText = bossText;
            _healthBar = bossHealthBar;
            _healthBarGameObject = _healthBar.gameObject;

            _bossText.SetActive(false);
            _healthBarGameObject.SetActive(false);
        }

        public void OnBossArrival(BossArrivalEvent @event)
        {
            _bossText.SetActive(true);
            _healthBarGameObject.SetActive(true);
        }

        public void UpdateHealthPrecent(BossDamagedEvent @event) => _healthBar.value = @event.BossHealth;

        public void OnBossDefeat(BossDefeatEvent @event)
        {
            _bossText.SetActive(false);
            _healthBarGameObject.SetActive(false);
        }

        public void OnGameStop()
        {
            _bossText.SetActive(false);
            _healthBarGameObject.SetActive(false);
        }
    }
}

