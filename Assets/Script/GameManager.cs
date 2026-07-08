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

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;

    [Header("Mobile UI")]
    [SerializeField] private GameObject mobileControls;

    public int Coins => coins;
    public int ZombiesKilled => zombiesKilled;
    public bool GameEnded => gameEnded;
    public bool IsPaused => isPaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // GameManager baru dari Level 2 hanya dipakai untuk mengirim UI ke GameManager utama
            Instance.SetLevelUI(
                coinText,
                gameOverPanel,
                winPanel,
                mobileControls,
                zombiesToWin
            );

            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        SetupLevel();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Menu")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 1f;

            SetMobileControls(false);

            return;
        }

        SetupLevel();
    }

    private void SetLevelUI(
        TextMeshProUGUI newCoinText,
        GameObject newGameOverPanel,
        GameObject newWinPanel,
        GameObject newMobileControls,
        int newZombiesToWin
    )
    {
        coinText = newCoinText;
        gameOverPanel = newGameOverPanel;
        winPanel = newWinPanel;
        mobileControls = newMobileControls;
        zombiesToWin = newZombiesToWin;

        Debug.Log("UI level baru berhasil dipasang ke GameManager utama");

        SetupLevel();
    }

    private void SetupLevel()
    {
        Time.timeScale = 1f;

        gameEnded = false;
        isPaused = false;
        zombiesKilled = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);

        SetMobileControls(true);

        UpdateCoinUI();
    }

    private void SetMobileControls(bool active)
    {
        if (mobileControls != null)
            mobileControls.SetActive(active);
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = coins.ToString();
        }
        else
        {
            Debug.LogWarning("CoinText belum dipasang ke GameManager!");
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinUI();

        Debug.Log("Coin : " + coins);
    }

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

    public void PlayerDied()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        Debug.Log("GAME OVER");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("GameOverPanel belum dipasang ke GameManager!");
        }

        EndGame();
    }

    private void WinGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        Debug.Log("YOU WIN");

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("WinPanel belum dipasang ke GameManager!");
        }

        EndGame();
    }

    private void EndGame()
    {
        SetMobileControls(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void PauseGame()
    {
        if (gameEnded)
            return;

        isPaused = true;

        SetMobileControls(false);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        if (gameEnded)
            return;

        isPaused = false;

        SetMobileControls(true);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        gameEnded = false;
        isPaused = false;
        zombiesKilled = 0;

        SetMobileControls(true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        coins = 0;
        zombiesKilled = 0;
        gameEnded = false;
        isPaused = false;

        SetMobileControls(false);

        Time.timeScale = 1f;

        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        gameEnded = false;
        isPaused = false;
        zombiesKilled = 0;

        SetMobileControls(true);

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}