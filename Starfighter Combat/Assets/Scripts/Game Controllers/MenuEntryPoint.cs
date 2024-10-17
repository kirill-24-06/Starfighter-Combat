using UnityEngine;
using UnityEngine.Audio;

public class MenuEntryPoint : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    private AudioSettingsLoader _audioController;


    private void Start() => Initialise();
   
    private void Initialise()
    {
        //PlayerPrefs.DeleteAll();
        _audioController = new AudioSettingsLoader(_audioMixer).LoadConfig();
    }
}
