using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : IResetable
{
    private GameObject _bossText;

    private GameObject _healthBarGameObject;
    private Slider _healthBar;

    public BossHealthBar(GameObject bossText, Slider bossHealthBar)
    {
        _bossText = bossText;
        _healthBar = bossHealthBar;
        _healthBarGameObject = _healthBar.gameObject;

        EntryPoint.Instance.Events.BossArrival += OnBossArrival;
        EntryPoint.Instance.Events.BossDamaged += UpdateHealthPrecent;
        EntryPoint.Instance.Events.BossDefeated += OnBossDefeat;
        EntryPoint.Instance.Events.Stop += OnGameStop;

        _bossText.SetActive(false);
        _healthBarGameObject.SetActive(false);
    }

    private void OnBossArrival()
    {
        _bossText.SetActive(true);
        _healthBarGameObject.SetActive(true);
    }

    private void UpdateHealthPrecent(float bossHealth) => _healthBar.value = bossHealth;

    private void OnBossDefeat()
    {
        _bossText.SetActive(false);
        _healthBarGameObject.SetActive(false);
    }

    private void OnGameStop()
    {
        _bossText.SetActive(false);
        _healthBarGameObject.SetActive(false);
    }

    public void Reset()
    {
        EntryPoint.Instance.Events.BossArrival -= OnBossArrival;
        EntryPoint.Instance.Events.BossDamaged -= UpdateHealthPrecent;
        EntryPoint.Instance.Events.BossDefeated -= OnBossDefeat;
        EntryPoint.Instance.Events.Stop -= OnGameStop;
    }
}