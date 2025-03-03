using Refactoring.Ui.DialogWindows;
using Refactoring.Ui.DialogWindows.Dialogs;
using UnityEngine;

namespace Refactoring
{
    public class Bootstrap : MonoBehaviour
    {
        private void Start()
        {
            DialogManager.ShowDialog<WelcomeDialog>();
        }
    }
}
