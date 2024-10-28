using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject _heartsLayout;
    [SerializeField] private List<Image> _hearts;

    [SerializeField] private GameObject _healtImage;
    [SerializeField] private TextMeshProUGUI _healthText;
    private GameObject _healthTextGameObject;


    public void Initialise()
    {
        _healthTextGameObject = _healthText.gameObject;
       EntryPoint.Instance.Events.ChangeHealth += ShowHealth;
    }

    private void ShowHealth(int newHealth)
    {
        bool useLayout = newHealth <= 5;
        
        _heartsLayout.SetActive(useLayout);

        _healtImage.SetActive(!useLayout);
        _healthTextGameObject.SetActive(!useLayout);

        if (_healtImage.activeInHierarchy)
            _healthText.text = "X " + newHealth;
       
        else if (_heartsLayout.activeInHierarchy)
        {
            for (int i = 0; i < _hearts.Count; i++)
            {
                bool isHeartActive = i <= (newHealth - 1);
                _hearts[i].gameObject.SetActive(isHeartActive);
            }
        }
    }

    private void OnDestroy() => EntryPoint.Instance.Events.ChangeHealth -= ShowHealth;
}
