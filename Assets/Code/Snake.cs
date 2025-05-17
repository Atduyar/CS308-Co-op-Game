using UnityEngine;

public class Snake : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float groundCheckDistance = 1f;
    public LayerMask groundLayer;
    public bool looksLeftByDefault = true;

    private Vector2 moveDirection = Vector2.right;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            Debug.LogError("SpriteRenderer yok!");
        if (animator == null)
            Debug.LogError("Animator yok!");
    }

    void Update()
    {
        // Hareket ettir
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Altýnda zemin var mý?
        Vector2 rayOrigin = (Vector2)transform.position + new Vector2(moveDirection.x * 0.5f, 0);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckDistance, groundLayer);

        Debug.DrawRay(rayOrigin, Vector2.down * groundCheckDistance, Color.red); // Editor'da görsel kontrol

        if (hit.collider == null)
        {
            // Zemin yoksa yön deðiþtir
            moveDirection *= -1;
        }

        // Sprite yönü ayarla
        spriteRenderer.flipX = looksLeftByDefault ? moveDirection.x > 0 : moveDirection.x < 0;

        animator.SetBool("isWalking", true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RespawnController respawn = other.GetComponent<RespawnController>();
            if (respawn != null)
                respawn.Respawn();
        }
    }
}

