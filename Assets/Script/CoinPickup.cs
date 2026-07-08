using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("Coin Setting")]
    [SerializeField] private int coinValue = 1;

    [Header("Sound Effect")]
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private float soundVolume = 1f;

    private Vector3 startPos;
    private bool alreadyPicked = false;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.Rotate(0f, 120f * Time.deltaTime, 0f);

        transform.position = new Vector3(
            startPos.x,
            startPos.y + Mathf.Sin(Time.time * 2f) * 0.15f,
            startPos.z
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyPicked) return;

        if (!other.CompareTag("Player"))
            return;

        alreadyPicked = true;

        GameManager.Instance?.AddCoins(coinValue);

        if (coinSound != null)
        {
            AudioSource.PlayClipAtPoint(coinSound, transform.position, soundVolume);
        }

        Debug.Log("Coin berhasil diklaim! +" + coinValue);

        Destroy(gameObject);
    }
}