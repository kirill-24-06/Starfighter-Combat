using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _bossMusic;
    private AudioSource _musicPlayer;

    public void Initialise()
    {
        _musicPlayer = GetComponent<AudioSource>();
        EntryPoint.Instance.Events.Pause += OnPause;
        EntryPoint.Instance.Events.BossArrival += OnBossArrival;
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

    private void OnBossArrival()
    {
        _musicPlayer.clip = _bossMusic;
        _musicPlayer.Play();
    }

    private void OnDestroy()
    {
        EntryPoint.Instance.Events.Pause -= OnPause;
        EntryPoint.Instance.Events.BossArrival -= OnBossArrival;
    }
}
