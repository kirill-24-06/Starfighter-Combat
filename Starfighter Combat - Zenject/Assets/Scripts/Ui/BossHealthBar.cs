using UnityEngine;
using UnityEngine.UI;

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

    public void OnBossArrival()
    {
        _bossText.SetActive(true);
        _healthBarGameObject.SetActive(true);
    }

    public void UpdateHealthPrecent(float bossHealth) => _healthBar.value = bossHealth;

    public void OnBossDefeat()
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