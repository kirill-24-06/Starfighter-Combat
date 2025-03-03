using UnityEngine;

[CreateAssetMenu(fileName = "new Audio Clips", menuName = "ScriptableObjects/Level/AudioClips")]
public class AudioClips : ScriptableObject
{
    [field: SerializeField] public AudioClip LevelMusic { get; private set; }
    [field: SerializeField] public AudioClip BossMusic { get; private set; }
}
