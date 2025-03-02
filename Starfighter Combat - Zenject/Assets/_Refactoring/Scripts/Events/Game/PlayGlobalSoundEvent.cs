using UnityEngine;
using Utils.Events.Channel.Static;

namespace Refactoring
{
    public struct PlayGlobalSoundEvent: IEvent
    {
        public AudioClip Sound;
        public float SoundVolume;

        public PlayGlobalSoundEvent(AudioClip sound, float volume)
        {
            Sound = sound;
            SoundVolume = volume;
        }
    }
}

