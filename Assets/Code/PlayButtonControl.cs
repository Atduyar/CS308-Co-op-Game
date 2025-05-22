using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayButtonControl : MonoBehaviour
{
    public Button playButton;

    void Start()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "userkey.txt");

        if (!File.Exists(filePath) || string.IsNullOrEmpty(File.ReadAllText(filePath)))
        {
            playButton.interactable = false;

            // Gri renk ayarla
            ColorBlock colors = playButton.colors;
            colors.normalColor = Color.gray;
            colors.highlightedColor = Color.gray;
            colors.pressedColor = Color.gray;
            colors.selectedColor = Color.gray;
            playButton.colors = colors;
        }
    }
}