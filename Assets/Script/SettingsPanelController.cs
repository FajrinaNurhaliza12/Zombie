using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    [Header("Volume / SFX Icon")]
    public Image volumeImage;
    public Sprite volumeOnSprite;
    public Sprite volumeOffSprite;

    [Header("Music / BGM Icon")]
    public Image musicImage;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    private void Start()
    {
        RefreshIcons();
        ApplyAllAudioSettings();
    }

    public void ToggleVolume()
    {
        GameSettings.SfxOn = !GameSettings.SfxOn;

        RefreshIcons();
        ApplyAllAudioSettings();
    }

    public void ToggleMusic()
    {
        GameSettings.MusicOn = !GameSettings.MusicOn;

        RefreshIcons();
        ApplyAllAudioSettings();
    }

    private void RefreshIcons()
    {
        if (volumeImage != null)
            volumeImage.sprite = GameSettings.SfxOn ? volumeOnSprite : volumeOffSprite;

        if (musicImage != null)
            musicImage.sprite = GameSettings.MusicOn ? musicOnSprite : musicOffSprite;
    }

    private void ApplyAllAudioSettings()
    {
        AudioCategory[] audioCategories = FindObjectsByType<AudioCategory>(FindObjectsSortMode.None);

        foreach (AudioCategory audioCategory in audioCategories)
        {
            audioCategory.ApplySetting();
        }
    }
}