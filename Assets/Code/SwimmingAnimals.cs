using UnityEngine;

public class SwimmingAnimals : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float checkDistance = 0.7f;
    public float checkRadius = 0.2f;
    public LayerMask waterLayer;
    public bool looksLeftByDefault = true;

    private Vector2 moveDirection;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float checkCooldown = 0.2f;
    private float checkTimer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            Debug.LogError("SpriteRenderer not found!");
        if (animator == null)
            Debug.LogError("Animator not found!");

        moveDirection = looksLeftByDefault ? Vector2.left : Vector2.right;
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        checkTimer -= Time.deltaTime;

        if (checkTimer <= 0f)
        {
            Vector2 checkPosition = (Vector2)transform.position + moveDirection * checkDistance;
            bool waterDetected = Physics2D.OverlapCircle(checkPosition, checkRadius, waterLayer);

            if (!waterDetected)
            {
                moveDirection *= -1f;
                FlipSprite();
                checkTimer = checkCooldown;
            }

            Debug.DrawLine(transform.position, checkPosition, Color.green, 0.1f);
        }

        animator.SetBool("isSwimming", true);
    }

    private void FlipSprite()
    {
        spriteRenderer.flipX = looksLeftByDefault ? moveDirection.x > 0 : moveDirection.x < 0;
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
