using System;
using UnityEngine;
using Zenject;
using Utils.Events.Channel.Static;

namespace Refactoring
{
    public class AudioPlayer : IInitializable, IDisposable
    {
        private AudioClips _audioClips;
        private AudioSource _musicPlayer;
        private AudioSource _globalSound;

        public AudioPlayer(AudioSource music, AudioSource globalSoundFX, AudioClips audioClips)
        {
            _musicPlayer = music;
            _audioClips = audioClips;
            _globalSound = globalSoundFX;

            Channel<PauseEvent>.OnEvent += OnPause;
            Channel<BossArrivalEvent>.OnEvent += OnBossArrival;
            Channel<PlayGlobalSoundEvent>.OnEvent += PlayGlobalSound;
        }

        public void Initialize()
        {
            _musicPlayer.clip = _audioClips.LevelMusic;
            _musicPlayer.Play();
        }

        private void OnPause(PauseEvent @event)
        {
            if (@event.Value)
            {
                _musicPlayer.Pause();
            }

            else
            {
                _musicPlayer.Play();
            }
        }

        private void OnBossArrival(BossArrivalEvent @event)
        {
            _musicPlayer.clip = _audioClips.BossMusic;
            _musicPlayer.Play();
        }

        private void PlayGlobalSound(PlayGlobalSoundEvent @event) => _globalSound.PlayOneShot(@event.Sound, @event.SoundVolume);

        public void Dispose()
        {
            Channel<PauseEvent>.OnEvent -= OnPause;
            Channel<BossArrivalEvent>.OnEvent -= OnBossArrival;
            Channel<PlayGlobalSoundEvent>.OnEvent -= PlayGlobalSound;
        }
    }
}