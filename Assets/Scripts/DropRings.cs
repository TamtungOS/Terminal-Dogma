using UnityEngine;

public class DropRings : MonoBehaviour
{
    [SerializeField] private float lifetime = 4f;        // seconds before it disappears
    [SerializeField] private float pickupDelay = 0.5f;   // seconds before it can be collected
    [SerializeField] private float pickupDistance = 1f;  // how close the player must be

    private SimplePool originPool;
    private PlayerHealth playerHealth;
    private Rigidbody2D rigidBody;
    private float timer = 0f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Called by PlayerHealth each time this ring is thrown out
    public void Launch(SimplePool pool, PlayerHealth player, Vector2 velocity)
    {
        originPool = pool;
        playerHealth = player;

        // Restart the timer because this ring may have been used before
        timer = 0f;

        rigidBody.linearVelocity = velocity;
    }

    private void Update()
    {
        timer = timer + Time.deltaTime;

        if (timer >= lifetime)
        {
            // Time ran out, so put the ring back in the pool
            originPool.ReturnToPool(gameObject);
        }
        else if (timer >= pickupDelay)
        {
            // The ring can be collected once the player is close enough
            float distance = Vector2.Distance(transform.position, playerHealth.transform.position);

            if (distance <= pickupDistance)
            {
                playerHealth.AddRing();
                originPool.ReturnToPool(gameObject);
            }
        }
    }
}