using UnityEngine;

public static class GameSettings
{
    private const string SFX_KEY = "SFX_ON";
    private const string MUSIC_KEY = "MUSIC_ON";

    public static bool SfxOn
    {
        get => PlayerPrefs.GetInt(SFX_KEY, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(SFX_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool MusicOn
    {
        get => PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(MUSIC_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}