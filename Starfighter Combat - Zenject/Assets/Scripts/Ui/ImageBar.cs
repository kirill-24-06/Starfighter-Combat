using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ImageBar
{
    protected GameObject _elementsLayout;
    protected List<Image> _elements;

    protected GameObject _textGO;
    protected TextMeshProUGUI _elementsTextDynamic;
    protected GameObject _elementsTextGO;

    public ImageBar(ImageBarElements barElements)
    {
        _elementsLayout = barElements.ElementsLayout;
        _elements = barElements.Elements;
        _textGO = barElements.TextGO;
        _elementsTextDynamic = barElements.ElementsTextDynamic;
        _elementsTextGO = barElements.ElementsTextGO;
    }

    public abstract void Show(int amount);
}