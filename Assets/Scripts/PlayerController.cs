using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Animator parameter names, looked up once and reused
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsGroundedHash = Animator.StringToHash("isGrounded");
    private static readonly int DoubleJumpHash = Animator.StringToHash("DoubleJump");
    private static readonly int RunSpeedMultiplierHash = Animator.StringToHash("RunSpeedMultiplier");

    private Vector2 moveInput;
    private Rigidbody2D rigidBody;
    private CapsuleCollider2D capsuleCollider; // Reference to player's body collider
    private Animator animator;
    private TrailRenderer speedTrail;
    private bool facingRight = true;

    [Header("Running")]
    [SerializeField] private float minSpeed = 6f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 30f;
    private float currentSpeed = 6f;
    [SerializeField] private float maxRunAnimationSpeed = 5f;

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int maxJumps = 2; // 1 = Single Jump, 2 = Double Jump
    private int jumpsRemaining;
    [SerializeField] private float coyoteTime = .1f;
    private float coyoteTimer = 0f;

    [Header("Ground Check Settings")]
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        speedTrail = GetComponent<TrailRenderer>();
    }

    private void Flip()
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
            if (coyoteTimer > 0f)
            {
                ExecuteJump();
                coyoteTimer = 0f;
            }
            else if (jumpsRemaining > 0)
            {
                // Jumping in the air uses up one air jump
                ExecuteJump();
                jumpsRemaining--;
                animator.SetTrigger(DoubleJumpHash);
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

        isGrounded = CheckIfGrounded();

        if (isGrounded && rigidBody.linearVelocity.y <= 0.01f)
        {
            // On the ground and not rising: refill the air jump and the coyote timer
            jumpsRemaining = maxJumps - 1;
            coyoteTimer = coyoteTime;
        }
        else
        {
            // In the air: count the coyote timer down
            coyoteTimer = coyoteTimer - Time.deltaTime;
        }

        // Tell the Animator what the player is doing
        animator.SetFloat(SpeedHash, Mathf.Abs(moveInput.x));
        animator.SetBool(IsGroundedHash, isGrounded);

        float speedPercent = Mathf.InverseLerp(minSpeed, maxSpeed, currentSpeed);
        float runAnimationSpeed = Mathf.Lerp(1f, maxRunAnimationSpeed, speedPercent * speedPercent);

        animator.SetFloat(RunSpeedMultiplierHash, runAnimationSpeed);
        speedTrail.emitting = speedPercent > 0.8f;
    }

    public void HeadStompBounce()
    {
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
        jumpsRemaining = maxJumps - 1;
    }

    private bool CheckIfGrounded()
    {
        // 1. Find the bottom center point of the CapsuleCollider2D
        Vector2 boxBottomCenter = new Vector2(capsuleCollider.bounds.center.x, capsuleCollider.bounds.min.y);

        // 2. Define a small box size (matching the width of the player, but only 0.1 units tall)
        Vector2 checkSize = new Vector2(capsuleCollider.bounds.size.x * 0.9f, 0.1f);

        // 3. Look for ground directly under the feet
        Collider2D hit = Physics2D.OverlapBox(boxBottomCenter, checkSize, 0f, groundLayer);

        return hit != null;
    }

    private void FixedUpdate()
    {
        float targetSpeed;
        float rate;

        // Lose built-up speed when the player is stopped, for example against a wall
        if (Mathf.Abs(rigidBody.linearVelocity.x) < 0.1f)
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