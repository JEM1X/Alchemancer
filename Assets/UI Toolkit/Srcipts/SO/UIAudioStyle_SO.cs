using UnityEngine;

[CreateAssetMenu(fileName = "UIAudioStyle", menuName = "Scriptable Objects/UI/UIAudioStyle_SO")]
public class UIAudioStyle_SO : ScriptableObject
{
    public AudioClip[] SoundList { get => soundList; }
    [SerializeField] private AudioClip[] soundList;
}
