using UnityEngine;
using UnityEngine.InputSystem;
using NativeWebSocket;

public class PlayerMovement : MonoBehaviour
{
    WebSocket websocket;

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

        WebSocketStart();
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


    // ---------- WebSocket Part
    async void WebSocketStart()
    {
        websocket = new WebSocket("wss://aaa.evrenomi.com");
        //websocket = new WebSocket("ws://localhost:8080");
        websocket.OnOpen += () =>
        {
            string joinJOSN = "{\"type\": \"join\",\"lobbyId\": "+ MainManager.Instance.lobbyId + "}";

            websocket.SendText(joinJOSN);
            Debug.Log("Connection open!\n"+joinJOSN);
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            Debug.Log("OnMessage!");
            Debug.Log(bytes);
            Debug.Log(bytes.ToString());

            // getting the message as a string
            // var message = System.Text.Encoding.UTF8.GetString(bytes);
            // Debug.Log("OnMessage! " + message);
        };

        // Keep sending messages at every 0.3s
        //InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        // waiting for messages
        await websocket.Connect();

        //MainManager.Instance.lobbyId;
    }
}
