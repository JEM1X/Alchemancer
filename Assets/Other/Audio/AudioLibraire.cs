using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibraire", menuName = "Scriptable Objects/AudioLibraire")]
public class AudioLibraire : ScriptableObject
{
    public AudioClip[] music;
    public AudioClip[] uiSounds;
    public AudioClip[] cardSounds;
    public AudioClip[] potionSounds;
    public AudioClip[] punchSounds;
    public AudioClip[] guideSounds;
}
