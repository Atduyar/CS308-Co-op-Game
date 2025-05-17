using UnityEngine;

public class PenguinPatrol : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float groundCheckDistance = 1f;
    public LayerMask groundLayer;
    public bool looksLeftByDefault = false;

    private Vector2 moveDirection = Vector2.right;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            Debug.LogError("SpriteRenderer not found!");
        if (animator == null)
            Debug.LogError("Animator not found!");
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        Vector2 rayOrigin = (Vector2)transform.position + new Vector2(moveDirection.x * 0.5f, 0);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckDistance, groundLayer);

        Debug.DrawRay(rayOrigin, Vector2.down * groundCheckDistance, Color.red);

        if (hit.collider == null)
            moveDirection *= -1;

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
            else
                Debug.LogWarning("RespawnController not found on Player!");
        }
    }
}
