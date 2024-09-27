using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool _hasCollided = false;

    public Rigidbody2D bulletRigidbody;
    
    public float bulletSpeed = 7;
    public int bulletDamage = 5;

    [HideInInspector] public int bulletDirection = 1;

    // Maximum distance the bullet can travel before being destroyed
    public float maxDistance = 10.0f;

    private Vector3 _spawnPosition;

    private void Start()
    {
        bulletRigidbody.velocity = new Vector3(bulletSpeed * bulletDirection, 0, 0);

        // Store the spawn position
        _spawnPosition = transform.position;
    }

    private void Update()
    {
        float distanceTraveled = Vector3.Distance(_spawnPosition, transform.position);

        // Destroy the bullet if it exceeds the maximum distance
        if (distanceTraveled > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasCollided) return;

        // Check if the layer of the collider is not "Enemy"
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy") && collision.gameObject.layer != LayerMask.NameToLayer("Map Border"))
        {
            return;
        }

        _hasCollided = true;

        Health enemyHealth = collision.GetComponent<Health>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }

}
