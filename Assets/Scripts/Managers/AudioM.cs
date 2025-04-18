using UnityEngine;

public class AudioM : Singleton<AudioM>
{
    [SerializeField] private AudioSource audioSource;

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
