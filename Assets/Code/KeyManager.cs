using UnityEngine;
using TMPro;
using System.IO;

public class KeyManager : MonoBehaviour
{
    public TMP_InputField keyInputField;
    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "userkey.txt");
    }

    public void SaveAndLogin()
    {
        string inputKey = keyInputField.text;

        if (string.IsNullOrEmpty(inputKey))
        {
            Debug.LogWarning("Lütfen bir key girin.");
            return;
        }

       
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, inputKey);
            Debug.Log("Ýlk kez giriþ yapýldý, key kaydedildi: " + inputKey);
            return;
        }

        
        string savedKey = File.ReadAllText(filePath);
        if (inputKey == savedKey)
        {
            Debug.Log("Key doðru, giriþ baþarýlý!");
            
            // SceneManager.LoadScene("Level1");
        }
        else
        {
            Debug.LogWarning("Key yanlýþ, giriþ reddedildi.");
        }
    }
}