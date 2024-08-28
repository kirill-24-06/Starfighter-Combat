using UnityEngine;
using UnityEngine.UI;
using Ui.DialogWindows;

public class PauseMenuDialog : Dialog
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _mainMenuButton;

    private void Start()
    {
        _continueButton.onClick.AddListener(Continue);
        _settingsButton.onClick.AddListener(GoToSettings);
        _mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void Continue()
    {
        EntryPoint.Instance.GameController.PauseGame(false);
        Hide();
    }

    private void GoToSettings()
    {
        Debug.Log("Work in progress!!!");
    }

    private void GoToMainMenu()
    {
        EntryPoint.Instance.Events.MainMenuExit?.Invoke();  
    }
}