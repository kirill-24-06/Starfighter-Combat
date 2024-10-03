using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ExplosionSound : MonoBehaviour
{
    private AudioSource _audioSource;
    private AudioClip _clip;
    private float _clipVolume;

    private void Awake()
    {
        GetComponent<AudioSource>();
    }

    public ExplosionSound Initialize(AudioClip clip, float clipVolume)
    {
        _clip = clip;
        _clipVolume = clipVolume;
        return this;
    }

    public ExplosionSound Play()
    {
        _audioSource.PlayOneShot(_clip, _clipVolume);

        Destroy(this, _clip.length + 1.0f);

        return this;
    }
}
