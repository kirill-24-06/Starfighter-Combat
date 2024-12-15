using System;
using UnityEngine;
using Zenject;

public class MusicPlayer : IInitializable, IDisposable
{
    private AudioClips _audioClips;
    private AudioSource _musicPlayer;

    private EventManager _events;

    public MusicPlayer(AudioSource music, AudioClips audioClips, EventManager events)
    {
        _musicPlayer = music;
        _audioClips = audioClips;

        _events = events;
        _events.Pause += OnPause;
        _events.BossArrival += OnBossArrival;
    }

    public void Initialize()
    {
        _musicPlayer.clip = _audioClips.LevelMusic;
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
        _musicPlayer.clip = _audioClips.BossMusic;
        _musicPlayer.Play();
    }

    public void Dispose()
    {
        _events.Pause -= OnPause;
        _events.BossArrival -= OnBossArrival;
    }
}
