using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip selectSound;
    public GameObject lobiPanel;
    public GameObject loginPanel;
    public GameObject creditsPanel;
    public GameObject mainMenuPanel;


    public void Start()
    {
        ShowMainMenu();
    }

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

    public void QuitGame()
    {
        AudioSource.PlayClipAtPoint(selectSound, transform.position);
        Application.Quit();
    }
    public void ShowLoginPanel()
    {
        AudioSource.PlayClipAtPoint(selectSound, transform.position);
        mainMenuPanel.SetActive(false);
        loginPanel.SetActive(true);
        lobiPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        AudioSource.PlayClipAtPoint(selectSound, transform.position);
        mainMenuPanel.SetActive(true);
        loginPanel.SetActive(false);
        lobiPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
    public void ShowCreditsMenu()
    {
        AudioSource.PlayClipAtPoint(selectSound, transform.position);
        mainMenuPanel.SetActive(false);
        loginPanel.SetActive(false);
        lobiPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }
    public void ShowLobiMenu()
    {
        AudioSource.PlayClipAtPoint(selectSound, transform.position);
        mainMenuPanel.SetActive(false);
        loginPanel.SetActive(false);
        lobiPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }
}