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
        Debug.Log("Dosya yolu: " + filePath);
    }

    public void SaveAndLogin()
    {
        string inputKey = keyInputField.text;

        if (string.IsNullOrEmpty(inputKey))
        {
            Debug.LogWarning("L�tfen bir key girin.");
            return;
        }

        
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, inputKey);
            Debug.Log("�lk kez giri� yap�ld�, key kaydedildi: " + inputKey);
            return;
        }

       
        string savedKey = File.ReadAllText(filePath);
        if (inputKey == savedKey)
        {
            Debug.Log("Key do�ru, giri� ba�ar�l�!");
            
           
        }
        else
        {
            Debug.LogWarning("Key yanl��, giri� reddedildi.");
        }
    }
}