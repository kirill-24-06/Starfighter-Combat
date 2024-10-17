using System;
using System.Collections.Generic;
using Ui.DialogWindows.Dialogs;
using UnityEngine;

namespace Ui.DialogWindows
{
    //Грузит префабы диалоговых окон из папки Resources
    public class DialogManager
    {
        private const string PrefabsFilePath = "Dialogs/";

        //Ссылки на префабы диалоговых окон в папке Assets/Resources/Dialogs/
        private static readonly Dictionary<Type, string> PrefabsDictionary = new Dictionary<Type, string>()
        {
            {typeof(GameOverWindow),"GameOverDialog"},
            {typeof(PauseMenuDialog),"PauseMenuDialog"},
            {typeof(YouWinDialog),"YouWinDialog"},
            {typeof(MainMenuDialog),"MainMenuDialog"},
            {typeof(SettingsDialog),"SettingsDialog" },
            {typeof(WelcomeDialog), "WelcomeDialog" }

        };

        public static T ShowDialog<T>() where T : Dialog
        {
            var dialog = GetPrefabByType<T>();
            if (dialog == null)
            {
                Debug.LogError("Show window - object not found");
                return null;
            }

            return GameObject.Instantiate(dialog, GuiHolder);
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

        //Canvas к которому крепятся диалоговые окна
        public static Transform GuiHolder
        {
            get
            {
                if (EntryPoint.Instance != null)
                {
                    return EntryPoint.Instance.UiRoot;
                }

                else
                {
                    return GameObject.FindObjectOfType<UiRoot>().transform;
                }
            }
        }
    }
}

