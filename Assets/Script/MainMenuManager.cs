using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject buttons;
    public GameObject settingsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OpenSettings()
    {
        Debug.Log("Settings Button Clicked");

        // Sembunyikan menu utama
        if (buttons != null)
            buttons.SetActive(false);

        // Tampilkan panel settings
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        // Tutup panel settings
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // Tampilkan kembali menu utama
        if (buttons != null)
            buttons.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}