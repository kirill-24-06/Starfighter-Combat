using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Refactoring.Ui.DialogWindows
{
    public abstract class Dialog : MonoBehaviour
    {
        [SerializeField] private Button _outsideClickArea;

        [SerializeField] protected GameObject _firstSelected;

        protected virtual void Awake()
        {
            if (_outsideClickArea != null)
            {
                _outsideClickArea.onClick.AddListener(Hide);
            }

            EventSystem.current.SetSelectedGameObject(_firstSelected);
        }

        protected void Hide()
        {
            EventSystem.current.SetSelectedGameObject(null);
            Destroy(gameObject);
        }

        protected void OnDestroy()
        {
            if (_outsideClickArea != null)
            {
                _outsideClickArea.onClick.RemoveAllListeners();
            }
        }
    }
}

