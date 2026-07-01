using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial UI")]
    [SerializeField] private GameObject tutorialPanel;

    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button skipButton;

    [Header("Countdown Text")]
    [SerializeField] private TextMeshProUGUI startCountdownText;

    [Header("Mobile UI")]
    [SerializeField] private GameObject mobileControls;

    [Header("Timer Settings")]
    [SerializeField] private float startDelay = 35f;

    private float timer;
    private bool canStart = false;
    private bool tutorialActive = false;

    private void Start()
    {
        ShowTutorial();
    }

    private void Update()
    {
        if (!tutorialActive)
            return;

        // Pastikan control mobile tetap hilang selama tutorial tampil
        if (mobileControls != null)
            mobileControls.SetActive(false);

        if (canStart)
            return;

        timer -= Time.unscaledDeltaTime;

        if (timer > 0)
        {
            if (startCountdownText != null)
                startCountdownText.text = "MULAI aktif dalam " + Mathf.CeilToInt(timer) + " detik";
        }
        else
        {
            canStart = true;

            if (startButton != null)
                startButton.interactable = true;

            if (startCountdownText != null)
                startCountdownText.text = "Klik MULAI untuk bermain";
        }
    }

    private void ShowTutorial()
    {
        tutorialActive = true;
        canStart = false;
        timer = startDelay;

        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);

        if (mobileControls != null)
            mobileControls.SetActive(false);

        if (startButton != null)
            startButton.interactable = false;

        if (skipButton != null)
            skipButton.interactable = true;

        if (startCountdownText != null)
            startCountdownText.text = "MULAI aktif dalam " + Mathf.CeilToInt(timer) + " detik";

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        if (!canStart)
            return;

        CloseTutorial();
    }

    public void SkipTutorial()
    {
        CloseTutorial();
    }

    private void CloseTutorial()
    {
        tutorialActive = false;

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        if (mobileControls != null)
            mobileControls.SetActive(true);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}