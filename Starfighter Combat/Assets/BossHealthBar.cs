using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : IResetable
{
    private GameObject _bossHealthBar;
    private Slider _healthBar;

    public BossHealthBar(GameObject healthBar)
    {
        _bossHealthBar = healthBar;
        _healthBar = _bossHealthBar.GetComponentInChildren<Slider>();

        EntryPoint.Instance.Events.BossArrival += OnBossArrival;
        EntryPoint.Instance.Events.BossDamaged += UpdateHealthPrecent;
        EntryPoint.Instance.Events.BossDefeated += OnBossDefeat;
        EntryPoint.Instance.Events.Stop += OnGameStop;

        _bossHealthBar.SetActive(false);
    }

    private void OnBossArrival() => _bossHealthBar.SetActive(true);

    private void UpdateHealthPrecent(float bossHealth) => _healthBar.value = bossHealth;

    private void OnBossDefeat() => _bossHealthBar.SetActive(false);

    private  void OnGameStop() => _bossHealthBar.SetActive(false);

    public void Reset()
    {
        EntryPoint.Instance.Events.BossArrival -= OnBossArrival;
        EntryPoint.Instance.Events.BossDamaged -= UpdateHealthPrecent;
        EntryPoint.Instance.Events.BossDefeated -= OnBossDefeat;
        EntryPoint.Instance.Events.Stop -= OnGameStop;
    }
}