using UnityEngine;
using Ui.DialogWindows;
using UnityEngine.UI;

namespace Legacy
{
    public class LoadingDialog : Dialog
    {
        [SerializeField] private Image _loadingImage;

        public Image LoadingImage => _loadingImage;
    }
}
