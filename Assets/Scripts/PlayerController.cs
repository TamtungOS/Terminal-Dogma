using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    private Vector2 moveInput;
    [SerializeField] private float moveSpeed = 6f;
    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider; // Reference to player's body collider

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private int maxJumps = 2; // 1 = Single Jump, 2 = Double Jump
    private int jumpsRemaining;

    [Header("Ground Check Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float extraHeight = 0.05f; // Distance cast past the bottom edge
    private bool isGrounded;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>(); // Automatically fetch the box collider
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (isGrounded || jumpsRemaining > 0)
            {
                ExecuteJump();
            }
        }
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Attack");
        }
    }

    private void ExecuteJump()
    {
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
        jumpsRemaining--; 
    }

    private void Update()
    {
        // Check for ground using a precise downward box cast
        isGrounded = CheckIfGrounded();

        if (isGrounded)
        {
            jumpsRemaining = maxJumps - 1; 
        }
    }

    private bool CheckIfGrounded()
    {
        // 1. Find the bottom center point of your BoxCollider2D
        Vector2 boxBottomCenter = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y);
        
        // 2. Define a small box size (matching the width of the player, but only 0.1 units tall)
        Vector2 checkSize = new Vector2(boxCollider.bounds.size.x * 0.9f, 0.1f);

        // 3. Look for a collision skin directly under the feet
        Collider2D hit = Physics2D.OverlapBox(boxBottomCenter, checkSize, 0f, groundLayer);
        
        return hit != null;
    }

    private void FixedUpdate()
    {
        rigidBody.linearVelocity = new Vector2(moveInput.x * moveSpeed, rigidBody.linearVelocity.y);
    }
}
