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
        if (PauseMenu.IsPaused) return;
        
        if (value.isPressed)
        {
            if (isGrounded)
            {
                ExecuteJump();
                jumpsRemaining--;
            }
            else if (jumpsRemaining > 0)
            {
                ExecuteJump();
                jumpsRemaining--;
            }
        }
    }

    private void ExecuteJump()
    {
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
    }

    private void Update()
    {
        if (PauseMenu.IsPaused) return;

        if (moveInput.x > 0 && !facingRight)
        {
            Flip();
            currentSpeed = minSpeed;
        }
        else if (moveInput.x < 0 && facingRight)
        {
            Flip();    
            currentSpeed = minSpeed;
        }
        // Check for ground using a precise downward box cast
        isGrounded = CheckIfGrounded();

        if (isGrounded)
        {
            jumpsRemaining = maxJumps - 1; 
        }
        // Debug.Log(currentSpeed);
    }

    public void HeadStompBounce()
    {
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
        jumpsRemaining = maxJumps - 1;
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
        float targetSpeed;
        float rate;

        if (Mathf.Abs(rigidBody.linearVelocity.x) < 0.1)
        {
            currentSpeed = minSpeed;
        }
        if (moveInput.x != 0)
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