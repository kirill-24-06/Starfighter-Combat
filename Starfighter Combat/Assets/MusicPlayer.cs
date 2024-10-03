using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    private AudioSource _musicPlayer;

    public void Initialise()
    {
        _musicPlayer = GetComponent<AudioSource>();
        EntryPoint.Instance.Events.Pause += OnPause;
    }

    private void Start()
    {
        _musicPlayer.Play();
    }

    private void OnPause(bool isPaused)
    {
        if (isPaused)
        {
            _musicPlayer.Pause();
        }

        else
        {
            _musicPlayer.Play();
        }
    }

    private void OnDestroy()
    {
        EntryPoint.Instance.Events.Pause -= OnPause;
    }
}
