using UnityEngine;
using UnityEngine.Audio;


namespace Refactoring
{
    public class MenuBootstrap : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private FPSPreset _targetFPS;

        private void Start() => Initialise();

        private void Initialise()
        {
            GraphicsLoader.LoadPreset((int)_targetFPS);
            var audioController = new AudioSettingsLoader(_audioMixer).LoadConfig();
        }
    }
}

