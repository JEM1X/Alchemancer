using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioClip[] music;
    public AudioClip[] uiSounds;
    public AudioClip[] cardSounds;
    public AudioClip[] potionSounds;
    public AudioClip[] guideSounds;

    [SerializeField] private AudioSource audioSource;


    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }

    public void PlayOnLoop(AudioClip sound)
    {
        audioSource.Play();
    }
}
