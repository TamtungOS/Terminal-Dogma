using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    private float detectionRange = 8f;
    private Transform playerTransform;
    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    
    [Header("Ground Check Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float forwardOffset = 0.05f; // Distance cast past the bottom edge

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeHit();
        }
    }
    private bool CheckIfPathIsSafe(float moveDirectionX)
    {
        // Determine facing direction (-1 for left, 1 for right)
        float facingDir = Mathf.Sign(transform.localScale.x);

        // Position the check slightly ahead of the feet in the direction of movement
        Vector2 checkPosition = new Vector2(
            boxCollider.bounds.center.x + (facingDir * forwardOffset), 
            boxCollider.bounds.min.y - 0.05f
        );
        
        Vector2 checkSize = new Vector2(0.2f, 0.1f);

        // Check if there is ground ahead under the feet
        Collider2D hit = Physics2D.OverlapBox(checkPosition, checkSize, 0f, groundLayer);
        
        return hit != null;
    }

    private void FixedUpdate()
    {
        if (playerTransform == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;

            // Flip the enemy visually based on movement direction
            if (direction.x > 0.01f)
            {
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
            else if (direction.x < -0.01f)
            {
                transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }

            // Check if there is ground ahead before applying horizontal velocity
            if (CheckIfPathIsSafe(direction.x))
            {
                // Move horizontally while preserving vertical gravity velocity (rigidBody.linearVelocity.y)
                rigidBody.linearVelocity = new Vector2(direction.x * moveSpeed, rigidBody.linearVelocity.y);
            }
            else
            {
                // Stop horizontal movement at ledges, keep vertical gravity active
                rigidBody.linearVelocity = new Vector2(0f, rigidBody.linearVelocity.y);
            }
        }
        else
        {
            // Stop horizontal movement when out of range, preserve vertical gravity
            rigidBody.linearVelocity = new Vector2(0f, rigidBody.linearVelocity.y);
        }
    }
}
