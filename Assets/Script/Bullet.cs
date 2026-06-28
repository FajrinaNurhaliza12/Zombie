using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float damage = 10f;
    public float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ZombieAI zombie = collision.gameObject.GetComponent<ZombieAI>();

        if (zombie != null)
        {
            zombie.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}