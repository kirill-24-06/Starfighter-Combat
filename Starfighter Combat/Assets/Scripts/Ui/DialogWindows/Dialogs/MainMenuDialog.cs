using Ui.DialogWindows;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuDialog : Dialog
{
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _aboutButton;
    [SerializeField] private Button _exitGameButton;

    protected override void Awake()
    {
        base.Awake();

        _newGameButton.onClick.AddListener(StartNewGame);
        _settingsButton.onClick.AddListener(OpenSettings);
        _aboutButton.onClick.AddListener(ShowAboutInfo);
        _exitGameButton.onClick.AddListener(ExitGame);
    }

    private void StartNewGame()
    {
        SceneManager.LoadScene(0);
    }
    
    private void OpenSettings()
    {
        DialogManager.ShowDialog<SettingsDialog>();
        Hide();
    }

    private void ShowAboutInfo()
    {
        Debug.Log("Work in progres");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}