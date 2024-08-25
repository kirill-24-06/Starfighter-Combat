using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject _heartsLayout;
    [SerializeField] private List<Image> _hearts;

    [SerializeField] private GameObject _textHealthBar;
    [SerializeField] private TextMeshProUGUI _healthText;


    public void Initialise()
    {
        EventManager.GetInstance().ChangeHealth += ShowHealth;
    }

    private void ShowHealth(int newHealth)
    {
        bool canShow = newHealth <= 5;

        _heartsLayout.SetActive(canShow);
        _textHealthBar.SetActive(!canShow);

        if (_textHealthBar.activeInHierarchy)
        {
            _healthText.text = "X " + newHealth;
        }

        else if (_heartsLayout.activeInHierarchy)
        {
            for (int i = 0; i < _hearts.Count; i++)
            {
                bool isHeartActive = i <= (newHealth - 1);
                _hearts[i].gameObject.SetActive(isHeartActive);
            }
        }
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().ChangeHealth -= ShowHealth;
    }
}
