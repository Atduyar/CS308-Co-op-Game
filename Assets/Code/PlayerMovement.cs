using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    [Header("Movment")]
    public float moveSpeed = 5.0f;
    private Vector2 movement;

    [Header("Jump")]
    public float jumpPower = 10.0f;
    public int maxJump = 2;
    public int jumpRemaining = 0;

    [Header("Sound")]
    public AudioClip jumpPlayer;
    
    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float fallSpeedtMultiplier = 2f;
    public float maxFallSpeed = 20f;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
        GroundedCheck();
        Gravity();
    }

    public void Gravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedtMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        movement.x = ctx.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && jumpRemaining > 0)
        {
            AudioSource.PlayClipAtPoint(jumpPlayer, transform.position);
            rb.linearVelocityY = jumpPower;
            jumpRemaining--;
        }
        else if (ctx.canceled)
        {
            rb.linearVelocityY *= 0.5f;
        }
    }


    public void GroundedCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer)
            && rb.linearVelocity.y <= 0)
        {
            jumpRemaining = maxJump;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }
}
