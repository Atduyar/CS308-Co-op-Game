using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextIndex);

    }

    public void mainMenu()
    {

        SceneManager.LoadScene("Buttons");
    }
    public void settingsMenu()
    {
        SceneManager.LoadScene("SettingsMenu");

    }

    public void QuitGame()
    {

        Application.Quit();


    }
}