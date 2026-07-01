using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    // ===== UI HEALTH BAR =====
    [Header("UI")]
    [SerializeField] private Slider healthSlider;

    private void Start()
    {
        currentHealth = maxHealth;

        SetupHealthUI();

        UpdateHealthUI();

        Debug.Log("HP Player : " + currentHealth);
    }

    private void SetupHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        else
        {
            Debug.LogWarning("Health Slider belum diassign pada PlayerHealth!");
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(
            currentHealth - amount,
            0,
            maxHealth
        );

        UpdateHealthUI();

        Debug.Log("HP Player : " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("PLAYER MATI");

            GameManager.Instance?.PlayerDied();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(
            currentHealth + amount,
            0,
            maxHealth
        );

        UpdateHealthUI();

        Debug.Log("Heal : " + currentHealth);
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }
}