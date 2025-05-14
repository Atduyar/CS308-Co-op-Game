using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip selectSound;

    public void StartGame()
    {
        AudioSource.PlayClipAtPoint(selectSound, transform.position);   
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextIndex);

    }

    public void mainMenu()
    {
        AudioSource.PlayClipAtPoint(selectSound, transform.position);
        SceneManager.LoadScene("Buttons");
    }
    public void settingsMenu()
    {
        AudioSource.PlayClipAtPoint(selectSound, transform.position);
        SceneManager.LoadScene("SettingsMenu");

    }

    public void QuitGame()
    {

        AudioSource.PlayClipAtPoint(selectSound, transform.position);
        Application.Quit();

    }
}