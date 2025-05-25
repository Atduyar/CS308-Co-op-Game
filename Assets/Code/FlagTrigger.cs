using Unity.VisualScripting;
using UnityEngine;

public class FlagTrigger : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RespawnController respawn = collision.GetComponent<RespawnController>();
            gameManager.GameOver(respawn);
        }
    }
}
