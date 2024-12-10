using UnityEngine;
using UnityEngine.Audio;

public class MenuEntryPoint : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private FPSPreset _targetFPS;
    private AudioSettingsLoader _audioController;

    private void Start() => Initialise();
   
    private void Initialise()
    {
       GraphicsLoader.LoadPreset((int)_targetFPS);
        _audioController = new AudioSettingsLoader(_audioMixer).LoadConfig();
    }
}
