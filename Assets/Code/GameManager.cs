using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;

    private bool gameOver = false;

    public void GameOver()
    {
        if (gameOver) return;
        gameOver = true;

        Debug.Log("Game Over function called!");
        gameOverUI.SetActive(true);

        StartCoroutine(RestartAfterDelay(3f));

        Time.timeScale = 0f;
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
