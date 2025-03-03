using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Refactoring
{
    public abstract class ImageBar
    {
        protected GameObject _elementsLayout;
        protected List<Image> _elements;

        protected GameObject _imageGO;
        protected TextMeshProUGUI _elementsTextDynamic;
        protected GameObject _textGO;

        public ImageBar(
            GameObject elemetsLayout,
            List<Image> elements,
            GameObject imageGameObject,
            TextMeshProUGUI dynamicText,
            GameObject elementsTextGameObject
            )
        {
            _elementsLayout = elemetsLayout;
            _elements = elements;
            _imageGO = imageGameObject;
            _elementsTextDynamic = dynamicText;
            _textGO = elementsTextGameObject;
        }
    }
}

