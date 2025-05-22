using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayButtonController : MonoBehaviour
{
    public Button playButton;

    private string filePath;

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "userkey.txt");
        Refresh();
    }

    public void Refresh()
    {
        if (File.Exists(filePath))
        {
            string key = File.ReadAllText(filePath).Trim();

            if (!string.IsNullOrEmpty(key))
            {
                playButton.interactable = true;
                return;
            }
        }

        playButton.interactable = false;

        ColorBlock grayColors = playButton.colors;
        grayColors.normalColor = Color.gray;
        grayColors.highlightedColor = Color.gray;
        grayColors.pressedColor = Color.gray;
        playButton.colors = grayColors;
    }
}