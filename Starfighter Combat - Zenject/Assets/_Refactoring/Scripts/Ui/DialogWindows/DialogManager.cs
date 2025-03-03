using Refactoring.Ui.DialogWindows.Dialogs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Refactoring.Ui.DialogWindows
{
    public class DialogManager
    {
        public static DialogManager StaticInstance { get; private set; }
        private static Transform RootCanvas => StaticInstance.UiRoot;

        private const string PrefabsFilePath = "Dialogs/";

        private static readonly Dictionary<Type, string> PrefabsDictionary = new()
        {
            {typeof(GameOverWindow),"GameOverDialog"},
            {typeof(PauseMenuDialog),"PauseMenuDialog"},
            {typeof(YouWinDialog),"YouWinDialog"},
            {typeof(MainMenuDialog),"MainMenuDialog"},
            {typeof(SettingsDialog),"SettingsDialog" },
            {typeof(WelcomeDialog), "WelcomeDialog" },
            {typeof(LoadingDialog), "LoadingScreen" }

        };

        public Transform UiRoot;

        public DialogManager(Transform rootCanvas)
        {
            StaticInstance = this;
            UiRoot = rootCanvas;
        }


        public static T ShowDialog<T>() where T : Dialog
        {
            var dialog = GetPrefabByType<T>();
            if (dialog == null)
            {
                Debug.LogError("Show window - object not found");
                return null;
            }

            return GameObject.Instantiate(dialog, RootCanvas);
        }

        private static T GetPrefabByType<T>() where T : Dialog
        {
            var prefabName = PrefabsDictionary[typeof(T)];
            if (string.IsNullOrEmpty(prefabName))
            {
                Debug.LogError("cant find prefab type of " + typeof(T) + "Do you added it in PrefabsDictionary?");
            }

            var path = PrefabsFilePath + PrefabsDictionary[typeof(T)];
            var dialog = Resources.Load<T>(path);
            if (dialog == null)
            {
                Debug.LogError("Cant find prefab at path " + path);
            }

            return dialog;
        }

    }
}

