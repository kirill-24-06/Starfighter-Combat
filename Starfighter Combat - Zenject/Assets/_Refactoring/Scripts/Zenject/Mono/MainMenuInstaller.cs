using Refactoring.Ui.DialogWindows;
using Zenject;
using UnityEngine;

namespace Refactoring
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private UiRoot _root;

        public override void InstallBindings()
        {
            var dialogManager = new DialogManager(_root.transform);
            Container
                .Bind<DialogManager>()
                .FromInstance(dialogManager)
                .AsSingle()
                .NonLazy();
        }
    }

}
