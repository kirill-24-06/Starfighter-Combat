using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Refactoring
{
    public class HealthBar : ImageBar
    {
        public HealthBar(
            GameObject elemetsLayout,
            List<Image> elements,
            GameObject imageGameObject,
            TextMeshProUGUI dynamicText,
            GameObject textGameObject) : base(elemetsLayout, elements, imageGameObject, dynamicText, textGameObject) { }
       
        public void Show(ChangeHealthEvent @event)
        {
            bool useLayout = @event.Health <= 5;

            _elementsLayout.SetActive(useLayout);

            _imageGO.SetActive(!useLayout);
            _textGO.SetActive(!useLayout);

            if (_imageGO.activeInHierarchy)
                _elementsTextDynamic.text = @event.Health.ToString();

            else if (_elementsLayout.activeInHierarchy)
            {
                for (int i = 0; i < _elements.Count; i++)
                {
                    bool isHeartActive = i <= (@event.Health - 1);
                    _elements[i].gameObject.SetActive(isHeartActive);
                }
            }
        }
    }
}

