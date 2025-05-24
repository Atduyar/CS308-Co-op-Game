using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public class WalkingEnemy : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip squishSound;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float groundCheckDistance = 1f;
    public LayerMask groundLayer;
    public bool looksLeftByDefault = false;

    [Header("Death Physics")]
    public float angularVelocityOnDeath = 360f;
    public float gravityScaleOnDeath = 1f;
    public float destroyDelay = 2f;

    private Vector2 moveDirection = Vector2.right;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D col;

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;

        if (!spriteRenderer) Debug.LogError("SpriteRenderer not found!");
        if (!animator) Debug.LogWarning("Animator not found (optional)");
    }

    void Update()
    {
        if (isDead) return;

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        Vector2 rayOrigin = (Vector2)transform.position + new Vector2(moveDirection.x * 0.5f, 0);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(rayOrigin, Vector2.down * groundCheckDistance, Color.red);

        if (hit.collider == null)
        {
            moveDirection *= -1;
        }

        spriteRenderer.flipX = looksLeftByDefault ? moveDirection.x > 0 : moveDirection.x < 0;
    }

    public void Die()
    {
        isDead = true;
        col.enabled = false;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravityScaleOnDeath;
        rb.angularVelocity = angularVelocityOnDeath;

        spriteRenderer.sortingOrder = 100;

        if (squishSound != null)
            AudioSource.PlayClipAtPoint(squishSound, transform.position, 10f);

        if (animator != null)
            animator.SetTrigger("Die");

        Destroy(gameObject, destroyDelay);
    }






    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            PlayerPower playerPower = collision.gameObject.GetComponent<PlayerPower>();
            if (playerPower.enemyKiller == true && contact.normal.y < -0.5f)
            {
                Die();


                PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
                if (player != null)
                    player.Bounce(10f);
            }
            else
            {
                
                RespawnController respawn = collision.gameObject.GetComponent<RespawnController>();
                if (respawn != null)
                    respawn.Respawn();
            }
        }
    }
}


