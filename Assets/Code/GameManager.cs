using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject startingChackpoint;

    private bool gameOver = false;

    public void GameOver(RespawnController rc)
    {
        if (gameOver) return;
        gameOver = true;

        Debug.Log("Game Over function called!");
        gameOverUI.SetActive(true);

        rc.chekpoint = startingChackpoint;

        StartCoroutine(RestartAfterDelay(rc, 3f));
    }

    private IEnumerator RestartAfterDelay(RespawnController rc, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        gameOver = false;
        gameOverUI.SetActive(false);
        rc.Respawn();
    }
}
