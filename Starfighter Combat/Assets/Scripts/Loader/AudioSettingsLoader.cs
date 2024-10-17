using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsLoader
{
    private AudioMixer _audioMixer;

    public AudioSettingsLoader(AudioMixer audioMixer)
    {
        _audioMixer = audioMixer;
    }

    public AudioSettingsLoader LoadConfig()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        float soundFXVolume = PlayerPrefs.GetFloat("Sound FX Volume", 0.6f);

        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        _audioMixer.SetFloat("Sound FX Volume", Mathf.Log10(soundFXVolume) * 20);

        return this;
    }
}
