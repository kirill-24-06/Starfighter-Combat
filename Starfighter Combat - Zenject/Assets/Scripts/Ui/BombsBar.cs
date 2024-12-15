public class BombsBar : ImageBar
{
    public BombsBar(ImageBarElements barElements) : base(barElements) { }
   
    public override void Show(int newBombsAmount)
    {
        bool useLayout = newBombsAmount <= 5;

        _elementsLayout.SetActive(useLayout);

        _textGO.SetActive(!useLayout);
        _elementsTextGO.SetActive(!useLayout);

        if (_textGO.activeInHierarchy)
            _elementsTextDynamic.text = newBombsAmount.ToString();

        else if (_elementsLayout.activeInHierarchy)
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                bool isHeartActive = i <= (newBombsAmount - 1);
                _elements[i].gameObject.SetActive(isHeartActive);
            }
        }
    }
}