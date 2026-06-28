using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Settings")]
    [SerializeField] private int zombiesToWin = 10;

    private int coins = 0;
    private int zombiesKilled = 0;
    private bool gameEnded = false;
    private bool isPaused = false;

    // =============================
    // UI ADDITION
    // =============================
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;

    // =============================
    // PROPERTY UNTUK UI
    // =============================
    public int Coins => coins;
    public int ZombiesKilled => zombiesKilled;
    public bool GameEnded => gameEnded;
    public bool IsPaused => isPaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // hide UI awal
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    //==================================
    // COIN
    //==================================

    public void AddCoins(int amount)
    {
        coins += amount;

        if (coinText != null)
            coinText.text = coins.ToString();

        Debug.Log("Coin : " + coins);
    }

    //==================================
    // ZOMBIE
    //==================================

    public void OnZombieKilled(int reward)
    {
        if (gameEnded)
            return;

        zombiesKilled++;

        AddCoins(reward);

        Debug.Log("Zombie : " + zombiesKilled + "/" + zombiesToWin);

        if (zombiesKilled >= zombiesToWin)
        {
            WinGame();
        }
    }

    //==================================
    // PLAYER
    //==================================

    public void PlayerDied()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        Debug.Log("GAME OVER");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        EndGame();
    }

    //==================================
    // WIN
    //==================================

    private void WinGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        Debug.Log("YOU WIN");

        if (winPanel != null)
            winPanel.SetActive(true);

        EndGame();
    }

    //==================================
    // END GAME
    //==================================

    private void EndGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    //==================================
    // PAUSE
    //==================================

    public void PauseGame()
    {
        if (gameEnded)
            return;

        isPaused = true;

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //==================================
    // SCENE
    //==================================

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // OPTIONAL NEXT LEVEL
    public void NextLevel()
    {
        Time.timeScale = 1f;

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }
}