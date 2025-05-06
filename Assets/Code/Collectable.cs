using UnityEngine;

public class Collectable : MonoBehaviour
{
    public CollectableEffect data;

    [SerializeField] private float destroyDelay = 0.5f;
    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected || data == null) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;

            data.Apply(other.gameObject);
            PlayFeedback();
            Destroy(gameObject, destroyDelay);
        }
    }
    private void PlayFeedback()
    {
        if (data.collectEffect != null)
        {
            Instantiate(data.collectEffect, transform.position, Quaternion.identity);
        }

        if (data.collectSound != null)
        {
            AudioSource.PlayClipAtPoint(data.collectSound, transform.position);
        }
    }
}
