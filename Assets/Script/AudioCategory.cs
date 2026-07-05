using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioCategory : MonoBehaviour
{
    public enum AudioType
    {
        SFX,
        Music
    }

    [Header("Audio Type")]
    public AudioType audioType;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ApplySetting();
    }

    private void OnEnable()
    {
        ApplySetting();
    }

    public void ApplySetting()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            return;

        if (audioType == AudioType.SFX)
        {
            audioSource.mute = !GameSettings.SfxOn;
        }
        else if (audioType == AudioType.Music)
        {
            audioSource.mute = !GameSettings.MusicOn;
        }
    }
}