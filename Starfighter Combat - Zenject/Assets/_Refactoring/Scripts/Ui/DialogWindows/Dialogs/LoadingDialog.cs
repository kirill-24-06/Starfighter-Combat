using UnityEngine;
using UnityEngine.UI;

namespace Refactoring.Ui.DialogWindows.Dialogs
{
    public class LoadingDialog : Dialog
    {
        [SerializeField] private Image _loadingImage;

        public Image LoadingImage => _loadingImage;
    }

}

