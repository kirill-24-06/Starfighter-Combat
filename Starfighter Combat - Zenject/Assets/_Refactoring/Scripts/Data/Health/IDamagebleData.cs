using UnityEngine;

namespace Refactoring
{
    public interface IDamagebleData
    {
        public GameObject Explosion { get; }
        public int Health { get;}
        public AudioClip ExplosionSound { get;}

        public float ExplosionSoundVolume { get; }
    }
}

