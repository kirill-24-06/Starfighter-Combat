using UnityEngine;
using UnityEngine.UI;

public abstract class Bar : MonoBehaviour
{
    [SerializeField] private Image _filler;

    protected void OnValueChange(float value) => _filler.fillAmount = value;
}
