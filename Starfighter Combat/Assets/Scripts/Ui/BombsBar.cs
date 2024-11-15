using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BombsBar : MonoBehaviour
{
    [SerializeField] private GameObject _bombsLayout;
    [SerializeField] private List<Image> _bombs;

    [SerializeField] private GameObject _textBombBar;
    [SerializeField] private TextMeshProUGUI _bombTextDynamic;
    [SerializeField] private GameObject _bombTextGameObject;


    public void Initialise()
    {
        EntryPoint.Instance.Events.BonusAmountUpdate += ShowBombs;
    }

    private void ShowBombs(int newBombsAmount)
    {
        bool useLayout = newBombsAmount <= 5;

        _bombsLayout.SetActive(useLayout);

        _textBombBar.SetActive(!useLayout);
        _bombTextGameObject.SetActive(!useLayout);

        if (_textBombBar.activeInHierarchy)
            _bombTextDynamic.text = newBombsAmount.ToString();

        else if (_bombsLayout.activeInHierarchy)
        {
            for (int i = 0; i < _bombs.Count; i++)
            {
                bool isHeartActive = i <= (newBombsAmount - 1);
                _bombs[i].gameObject.SetActive(isHeartActive);
            }
        }
    }

    private void OnDestroy()
    {
        EntryPoint.Instance.Events.BonusAmountUpdate -= ShowBombs;
    }
}
