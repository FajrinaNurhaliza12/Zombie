using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Health Setting")]
    [SerializeField] private float healAmount = 25f;

    [Header("Sound Effect")]
    [SerializeField] private AudioClip healthSound;
    [SerializeField] private float soundVolume = 1f;

    private Vector3 startPos;
    private bool alreadyPicked = false;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.Rotate(0f, 90f * Time.deltaTime, 0f);

        transform.position = new Vector3(
            startPos.x,
            startPos.y + Mathf.Sin(Time.time * 2f) * 0.2f,
            startPos.z
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyPicked) return;

        if (!other.CompareTag("Player")) return;

        alreadyPicked = true;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);

            if (healthSound != null)
            {
                AudioSource.PlayClipAtPoint(healthSound, transform.position, soundVolume);
            }

            Debug.Log("Medkit berhasil diklaim! HP bertambah +" + healAmount);
        }

        Destroy(gameObject);
    }
} 