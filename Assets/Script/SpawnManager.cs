using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [Header("Zombie Spawn")]
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private Transform[] zombieSpawnPoints;
    [SerializeField] private int totalZombie = 12;
    [SerializeField] private float zombieSpawnDelay = 5f;

    [Header("Coin Spawn")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform[] coinSpawnPoints;
    [SerializeField] private float coinSpawnDelay = 8f;
    [SerializeField] private int maxCoinOnMap = 4;

    [Header("First Aid Spawn")]
    [SerializeField] private GameObject firstAidPrefab;
    [SerializeField] private Transform[] firstAidSpawnPoints;
    [SerializeField] private float firstAidSpawnDelay = 12f;
    [SerializeField] private int maxFirstAidOnMap = 2;

    private int zombieCount = 0;
    private int coinCount = 0;
    private int firstAidCount = 0;

    // =========================
    // ADDED: GAME STATE CONTROL
    // =========================
    private bool isGameOver = false;

    private void Start()
    {
        StartCoroutine(SpawnZombieRoutine());
        StartCoroutine(SpawnCoinRoutine());
        StartCoroutine(SpawnFirstAidRoutine());
    }

    // =========================
    // ADDED: GAME OVER FUNCTION
    // =========================
    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER - Spawn dihentikan");
    }

    // =========================
    // ZOMBIE
    // =========================
    IEnumerator SpawnZombieRoutine()
    {
        while (!isGameOver && zombieCount < totalZombie)
        {
            yield return new WaitForSeconds(zombieSpawnDelay);

            if (zombiePrefab == null || zombieSpawnPoints.Length == 0)
                continue;

            Transform spawnPoint =
                zombieSpawnPoints[Random.Range(0, zombieSpawnPoints.Length)];

            Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);

            zombieCount++;

            Debug.Log($"Zombie Spawn : {zombieCount}/{totalZombie}");
        }
    }

    // =========================
    // COIN SYSTEM (SENDIRI)
    // =========================
    IEnumerator SpawnCoinRoutine()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(coinSpawnDelay);

            if (coinPrefab == null || coinSpawnPoints.Length == 0)
                continue;

            if (coinCount >= maxCoinOnMap)
                continue;

            Transform spawnPoint =
                coinSpawnPoints[Random.Range(0, coinSpawnPoints.Length)];

            GameObject coin = Instantiate(
                coinPrefab,
                spawnPoint.position,
                Quaternion.identity
            );

            coinCount++;

            ItemTracker tracker = coin.AddComponent<ItemTracker>();
            tracker.Init(() => coinCount--);

            Debug.Log($"COIN | {coinCount}/{maxCoinOnMap}");
        }
    }

    // =========================
    // FIRST AID SYSTEM (SENDIRI)
    // =========================
    IEnumerator SpawnFirstAidRoutine()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(firstAidSpawnDelay);

            if (firstAidPrefab == null || firstAidSpawnPoints.Length == 0)
                continue;

            if (firstAidCount >= maxFirstAidOnMap)
                continue;

            Transform spawnPoint =
                firstAidSpawnPoints[Random.Range(0, firstAidSpawnPoints.Length)];

            GameObject heal = Instantiate(
                firstAidPrefab,
                spawnPoint.position,
                Quaternion.identity
            );

            firstAidCount++;

            ItemTracker tracker = heal.AddComponent<ItemTracker>();
            tracker.Init(() => firstAidCount--);

            Debug.Log($"FIRST AID | {firstAidCount}/{maxFirstAidOnMap}");
        }
    }
}

// =========================
// ITEM TRACKER
// =========================
public class ItemTracker : MonoBehaviour
{
    private System.Action onDestroyed;

    public void Init(System.Action callback)
    {
        onDestroyed = callback;
    }

    private void OnDestroy()
    {
        onDestroyed?.Invoke();
    }
}