using UnityEngine;

[CreateAssetMenu(fileName ="Drone Data", menuName ="Config Data/Player/DefenceDrone",order =53)]
public class DefenceDroneData : ScriptableObject,IShooterData
{
    [SerializeField]private float _reloadCountDown;
    [SerializeField]private AudioClip _fireSound;
    [SerializeField, Range(0.1f, 1)] float _fireSoundVolume;

    public float ReloadCountDown => _reloadCountDown;

    public AudioClip FireSound => _fireSound;

    public float FireSoundVolume => _fireSoundVolume;
}