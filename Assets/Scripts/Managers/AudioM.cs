using UnityEngine;

public class AudioM : Singleton<AudioM>
{
    public AudioClip[] music;
    public AudioClip[] uiSounds;
    public AudioClip[] cardSounds;
    public AudioClip[] potionSounds;
    public AudioClip[] guideSounds;

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        audioSource.Play();
    }

    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }

    public void PlayOnLoop(AudioClip sound)
    {
        audioSource.resource = sound;
        audioSource.Play();
    }
}
