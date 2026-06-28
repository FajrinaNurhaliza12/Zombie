using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    // ===== TAMBAHAN UI =====
    [Header("UI")]
    [SerializeField] private Image healthFill;

    private void Start()
    {
        currentHealth = maxHealth;

        UpdateHealthUI();

        Debug.Log("HP Player : " + currentHealth);
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

    // ===== TAMBAHAN FUNCTION UI =====
    private void UpdateHealthUI()
    {
        if (healthFill != null)
        {
            float value = currentHealth / maxHealth;

            Debug.Log("FILL VALUE: " + value);

            healthFill.fillAmount = value;
        }
    }
}