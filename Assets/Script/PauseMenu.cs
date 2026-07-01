using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;

    [Header("Mobile UI")]
    [SerializeField] private GameObject mobileControls;

    private bool isPaused = false;

    private void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Pause Panel belum diassign pada Inspector.");
        }

        if (mobileControls != null)
            mobileControls.SetActive(true);
    }

    public void PauseGame()
    {
        if (pausePanel == null) return;

        isPaused = true;

        pausePanel.SetActive(true);

        if (mobileControls != null)
            mobileControls.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGame();
        }
        else
        {
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ResumeGame()
    {
        if (pausePanel == null) return;

        isPaused = false;

        pausePanel.SetActive(false);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResumeGame();
        }
        else
        {
            Time.timeScale = 1f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (mobileControls != null)
            mobileControls.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        isPaused = false;

        if (mobileControls != null)
            mobileControls.SetActive(true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;

        isPaused = false;

        if (mobileControls != null)
            mobileControls.SetActive(false);

        SceneManager.LoadScene("Main Menu");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        isPaused = false;

        if (mobileControls != null)
            mobileControls.SetActive(true);

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextScene < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.Log("Finish Game");
        }
    }
}