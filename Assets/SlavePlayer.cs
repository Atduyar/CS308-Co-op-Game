using UnityEngine;

public class SlavePlayer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector3 previousPosition;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        previousPosition = transform.position;
    }

    void Update()
    {
        FlipSprite();
        
        previousPosition = transform.position;
    }

    private void FlipSprite()
    {
        if (spriteRenderer == null) return;

        float horizontalMovement = transform.position.x - previousPosition.x;

        if (horizontalMovement > 0)
        {
            spriteRenderer.flipX = false; // right
        }
        else if (horizontalMovement < 0)
        {
            spriteRenderer.flipX = true; // left
        }
    }
}
