using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Refactoring
{
    public class BombsBar : ImageBar
    {
        public BombsBar(
            GameObject elemetsLayout,
            List<Image> elements,
            GameObject imageGameObject,
            TextMeshProUGUI dynamicText,
            GameObject textGameObject) : base(elemetsLayout, elements, imageGameObject, dynamicText, textGameObject) { }
      
        public void Show(BombsAmountUpdateEvent @event)
        {
            bool useLayout = @event.BombsAmount <= 5;

            _elementsLayout.SetActive(useLayout);

            _imageGO.SetActive(!useLayout);
            _textGO.SetActive(!useLayout);

            if (_imageGO.activeInHierarchy)
                _elementsTextDynamic.text = @event.BombsAmount.ToString();

            else if (_elementsLayout.activeInHierarchy)
            {
                for (int i = 0; i < _elements.Count; i++)
                {
                    bool isHeartActive = i <= (@event.BombsAmount - 1);
                    _elements[i].gameObject.SetActive(isHeartActive);
                }
            }
        }
    }
}

