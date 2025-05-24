using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float gravityScaleOnDeath = 1f;
    public float angularVelocityOnDeath = 360f;
    public float destroyDelay = 2f;

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private bool IsFrontHit(Collider2D playerCollider)
    {
        // Oyuncunun yönüne göre mermi hangi taraftan geldi kontrolü
        Transform playerTransform = playerCollider.transform;
        SpriteRenderer playerSprite = playerCollider.GetComponentInChildren<SpriteRenderer>();

        if (playerSprite == null) return false;

        float bulletX = transform.position.x;
        float playerX = playerTransform.position.x;
        bool playerLookingLeft = playerSprite.flipX;

        if (playerLookingLeft)
            return bulletX < playerX; // Soluna çarpıyorsa
        else
            return bulletX > playerX; // Sağına çarpıyorsa
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Player"))
        {
            RespawnController respawn = collision.GetComponent<RespawnController>();
            PlayerPower playerPower = collision.GetComponent<PlayerPower>();

            // Ön cepheden çarptıysa ve kalkan yoksa: mermi Die() olur
            if (IsFrontHit(collision))
            {
                if (playerPower != null && playerPower.bulletShield)
                {
                    Die();
                    return;
                }

                Die(); // Ön cepheden gelen mermiler döner düşer
                return;
            }

            // Arka/yukarı/aşağıdan geliyorsa: oyuncu ölür, mermi yok olmaz
            if (respawn != null)
                respawn.Respawn();
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    public void Die()
    {
        isDead = true;
        col.enabled = false;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravityScaleOnDeath;
        rb.angularVelocity = angularVelocityOnDeath;

        spriteRenderer.sortingOrder = 100;

        Destroy(gameObject, destroyDelay);
    }
}
