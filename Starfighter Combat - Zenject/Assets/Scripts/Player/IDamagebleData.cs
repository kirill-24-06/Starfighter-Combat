using UnityEngine;

public interface IDamagebleData
{
    public int Health { get; set; }

    public int MaxHealth { get; set; }

    public GameObject ExplosionPrefab { get; set; }

    public AudioClip ExplosionSound { get; set; }

    public float ExplosionSoundVolume { get; set; }
}