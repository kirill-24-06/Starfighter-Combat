using Cysharp.Threading.Tasks;
using Refactoring.Ui.DialogWindows;
using Refactoring.Ui.DialogWindows.Dialogs;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Refactoring
{
    public static class SceneLoader
    {
        private static Image _loadingImage;
        private static readonly int _transitionDelay = 1300;

        public static void LoadScene(string sceneName)
        {
            var loadingScreen = DialogManager.ShowDialog<LoadingDialog>();
            _loadingImage = loadingScreen.LoadingImage;

            LoadSceneAsync(sceneName, loadingScreen.destroyCancellationToken).Forget();
        }

        public static async UniTaskVoid LoadSceneAsync(string sceneName, CancellationToken token)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            while (!operation.isDone && !token.IsCancellationRequested)
            {
                _loadingImage.fillAmount = operation.progress;

                if (operation.progress >= 0.9f && !operation.allowSceneActivation)
                {
                    await UniTask.Delay(_transitionDelay, cancellationToken: token);
                    operation.allowSceneActivation = true;
                }

                await UniTask.Yield();
            }
        }
    }
}

