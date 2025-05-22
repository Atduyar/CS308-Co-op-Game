using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public AudioClip backgroundMusic;
    public Rigidbody2D rb;
    public Animator ani;

    [Header("Movement")]
    public float moveSpeed = 5.0f;
    Vector2 movement;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float fallSpeedMultiplier = 2f;
    public float maxFallSpeed = 20f;

    [Header("Jump")]
    public float jumpPower = 10.0f;
    public int maxJump = 2;
    public int jumpRemaining = 0;
    public float jumpBufferTime = 0.15f;
    float jumpBuffer;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    [Header("Sound")]
    public AudioClip jumpPlayer;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer == null)
            Debug.LogWarning("SpriteRenderer not found on PlayerMovement!");
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
        GroundedCheck();
        Gravity();
        UpdateAnimation();
        FlipCharacter();
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        movement.x = ctx.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (jumpRemaining <= 0)
            {
                jumpBuffer = Time.fixedTime;
                return;
            }
            PerformJump();
        }
        else if (ctx.canceled)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    public void PerformJump()
    {
        AudioSource.PlayClipAtPoint(jumpPlayer, transform.position);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        jumpRemaining--;
        ani.SetTrigger("Jump");
    }

    public void Bounce(float force)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
    }

    public void GroundedCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer) &&
            rb.linearVelocity.y <= 0.05f)
        {
            jumpRemaining = maxJump;
            if (jumpBuffer + jumpBufferTime >= Time.fixedTime)
            {
                PerformJump();
            }
        }
    }

    public void Gravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void UpdateAnimation()
    {
        ani.SetFloat("Magnitude", rb.linearVelocity.magnitude);
        ani.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void FlipCharacter()
    {
        if (movement.x > 0)
            spriteRenderer.flipX = false;
        else if (movement.x < 0)
            spriteRenderer.flipX = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }
}
