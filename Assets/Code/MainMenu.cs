using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Windows;

public class MainMenu : MonoBehaviour
{
    public AudioClip selectSound;
    public GameObject lobiPanel;
    public GameObject loginPanel;
    public GameObject creditsPanel;
    public GameObject mainMenuPanel;
    public TMP_InputField keyInputField;
    public Button playButton;
    public TMP_Text playText;

    private string filePath;
    public static string JwtKey;

    public void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "userkey.txt");
        Debug.Log("Save File Path: " + filePath);
        if (System.IO.File.Exists(filePath))
        {
            JwtKey = System.IO.File.ReadAllText(filePath);
        }
        else
        {
            JwtKey = null;
        }

        ShowMainMenu();
        RefreshLoginBtn();
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

    public void RefreshLoginBtn()
    {
        if (System.IO.File.Exists(filePath))
        {
            JwtKey = System.IO.File.ReadAllText(filePath).Trim();

            if (!string.IsNullOrEmpty(JwtKey))
            {
                playButton.interactable = true;
                playText.color = Color.white;
                return;
            }
        }

        // No Key
        playButton.interactable = false;
        playText.color = Color.gray;
    }

    public void Login()
    {
        string inputKey = keyInputField.text;

        // Invalid Key
        if (string.IsNullOrEmpty(inputKey))
        {
            Debug.LogWarning("Invalid key.");
            return;
        }

        inputKey = inputKey.Replace(" ", "");
        inputKey = inputKey.Replace("\r", "");
        inputKey = inputKey.Replace("\n", "");
        inputKey = inputKey.Replace("\t", "");

        // Save new key
        System.IO.File.WriteAllText(filePath, inputKey);
        Debug.Log("Key saved: " + inputKey);
        JwtKey = inputKey;
        RefreshLoginBtn();
        ShowMainMenu();
    }
}