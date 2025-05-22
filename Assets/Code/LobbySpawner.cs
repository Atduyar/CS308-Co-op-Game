using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbySpawner : MonoBehaviour
{
    public GameObject lobbyButtonPrefab;       
    public Transform lobbyListPanel;         
    public TMP_InputField newLobbyInput;      

    //fake api
    private string[] fakeApiLobbyNames = new string[]
    {
        "Turnuva-123",
        "Arkadas Oda",
        "Test Lobi",
        "Gizli Oda"
    };

    void Start()
    {
        Debug.Log("LobbySpawner Start() ÇALIÞTI");

        foreach (string lobby in fakeApiLobbyNames)
        {
            Debug.Log("Lobi geliyor: " + lobby);
            CreateLobbyButton(lobby);
        }
    }

    public void AddLobby()
    {
        string input = newLobbyInput.text.Trim();

        if (string.IsNullOrEmpty(input))
        {
            Debug.LogWarning("Lobi adý boþ olamaz.");
            return;
        }

        CreateLobbyButton(input);
        newLobbyInput.text = ""; 
    }

    private void CreateLobbyButton(string lobbyName)
    {
        GameObject btn = Instantiate(lobbyButtonPrefab, lobbyListPanel);
        btn.GetComponentInChildren<TMP_Text>().text = "JOIN TO " + lobbyName;

        string capturedName = lobbyName;

        btn.GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Katýlýnýyor: " + capturedName);
            SceneManager.LoadScene("Level1");
        });
    }
}