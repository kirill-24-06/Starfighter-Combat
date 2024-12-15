public class HealthBar : ImageBar
{
    public HealthBar(ImageBarElements barElements) : base(barElements) { }
   
    public override void Show(int newHealth)
    {
        bool useLayout = newHealth <= 5;
        
        _elementsLayout.SetActive(useLayout);

        _textGO.SetActive(!useLayout);
        _elementsTextGO.SetActive(!useLayout);

        if (_textGO.activeInHierarchy)
            _elementsTextDynamic.text = newHealth.ToString();
       
        else if (_elementsLayout.activeInHierarchy)
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                bool isHeartActive = i <= (newHealth - 1);
                _elements[i].gameObject.SetActive(isHeartActive);
            }
        }
    }
}