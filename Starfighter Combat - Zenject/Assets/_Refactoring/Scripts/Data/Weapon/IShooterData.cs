using UnityEngine;

public interface IShooterData
{
    public float ReloadCountDown { get; }

    public AudioClip FireSound { get; }

    public float FireSoundVolume { get; }

    public GameObject Projectile { get; }
}