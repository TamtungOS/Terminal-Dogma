using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    private Vector2 moveInput;
    private Rigidbody2D rigidBody;
    private CapsuleCollider2D capsuleCollider; // Reference to player's body collider
    private bool facingRight = true;

    [Header("Running")]
    [SerializeField] InputActionReference runActionReference;
    private float currentSpeed = 6f;
    [SerializeField] private float minSpeed = 6f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 30f;

    private bool isRunning = false;

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int maxJumps = 2; // 1 = Single Jump, 2 = Double Jump
    private int jumpsRemaining;

    [Header("Ground Check Settings")]
    [SerializeField] private LayerMask groundLayer;
        private bool isGrounded;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>(); // Automatically fetch the box collider
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 localscale = transform.localScale;
        localscale.x *= -1;
        transform.localScale = localscale;
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

    private void ExecuteJump()
    {
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
        jumpsRemaining--; 
    }

    private void Update()
    {
        if (PauseMenu.IsPaused) return;

        if (moveInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && facingRight)
        {
            Flip();    
        }
        // Check for ground using a precise downward box cast
        isGrounded = CheckIfGrounded();

        if (isGrounded)
        {
            jumpsRemaining = maxJumps - 1; 
        }
        // Debug.Log(currentSpeed);
    }

    private bool CheckIfGrounded()
    {
        // 1. Find the bottom center point of your CapsuleCollider2D
        Vector2 boxBottomCenter = new Vector2(capsuleCollider.bounds.center.x, capsuleCollider.bounds.min.y);
        
        // 2. Define a small box size (matching the width of the player, but only 0.1 units tall)
        Vector2 checkSize = new Vector2(capsuleCollider.bounds.size.x * 0.9f, 0.1f);

        // 3. Look for a collision skin directly under the feet
        Collider2D hit = Physics2D.OverlapBox(boxBottomCenter, checkSize, 0f, groundLayer);
        
        return hit != null;
    }

    private void FixedUpdate()
    {
        isRunning = runActionReference.action.IsPressed();

        float targetSpeed;
        float rate;

        if (isRunning)
        {
            targetSpeed = maxSpeed;
            rate = acceleration;
        }
        else
        {
            targetSpeed = minSpeed;
            rate = deceleration;
        }

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, rate * Time.fixedDeltaTime);

        rigidBody.linearVelocity = new Vector2(moveInput.x * currentSpeed, rigidBody.linearVelocity.y);
    }
}