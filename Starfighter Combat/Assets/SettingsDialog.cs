using Ui.DialogWindows;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsDialog : Dialog
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Button _backButton;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _soundFXSlider;

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        _masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1);


        _masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        _musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        _soundFXSlider.onValueChanged.AddListener(SetSoundFX);

        _backButton.onClick.AddListener(GoBack);
    }

    private void SetMasterVolume(float volume)
    {
        _mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("MasterVolume",volume);
    }

    private void SetMusicVolume(float volume)
    {
        _mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    private void SetSoundFX(float volume)
    {
        _mixer.SetFloat("Sound FX Volume", Mathf.Log10(volume) * 20);
    }

    private void GoBack()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            DialogManager.ShowDialog<PauseMenuDialog>();
            Hide();
        }
        else
        {
            DialogManager.ShowDialog<MainMenuDialog>();
            Hide();
        }
    }

}
