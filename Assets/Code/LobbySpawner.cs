using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class LobbySpawner : MonoBehaviour
{
    public GameObject lobbyButtonPrefab;       
    public Transform lobbyListPanel;         
    public TMP_InputField newLobbyInput;

    [Serializable]
    class LobbyArrayWrawpper
    {
        public List<Lobby> Items;
    }

    [Serializable]
    class Lobby
    {
        public int id;
        public string name;
        public string count;
        public bool canJoin;
    }

    void OnEnable()
    {
        RefreshLobbies();
    }

    IEnumerator GetLobbies()
    {
        //using (UnityWebRequest webRequest = UnityWebRequest.Get("http://localhost:8080/lobby"))
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://aaa.evrenomi.com/lobby"))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + MainMenu.JwtKey);
            webRequest.SetRequestHeader("Cache-Control", "no-cache, no-store, must-revalidate");

            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("UnityWebRequest: Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("UnityWebRequest: HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("UnityWebRequest:\nReceived: " + webRequest.downloadHandler.text);
                    string fixedJson = "{\"Items\":" + webRequest.downloadHandler.text + "}";
                    Debug.Log(fixedJson);
                    LobbyArrayWrawpper lobbies = JsonUtility.FromJson<LobbyArrayWrawpper>(fixedJson);
                    Debug.Log(lobbies.Items);
                    if (lobbies.Items == null)
                    {
                        break;
                    }
                    foreach (Lobby lobby in lobbies.Items)
                    {
                        CreateLobbyButton(lobby);
                    }
                    break;
            }
        }
    }
    IEnumerator CreateLobby(string name)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post("https://aaa.evrenomi.com/lobby", "{\"name\": \"" + name + "\"}", "application/json"))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + MainMenu.JwtKey);
            webRequest.SetRequestHeader("Cache-Control", "no-cache, no-store, must-revalidate");

            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("UnityWebRequest: Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("UnityWebRequest: HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("UnityWebRequest:\nReceived: " + webRequest.downloadHandler.text);
                    RefreshLobbies();
                    break;
            }
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
    
        StartCoroutine(CreateLobby(input));
    }
    void RefreshLobbies()
    {
        foreach (Transform transform in lobbyListPanel.transform)
        {
            Destroy(transform.gameObject);
        }
        StartCoroutine(GetLobbies());
    }

    private void CreateLobbyButton(Lobby lobby)
    {
        GameObject btn = Instantiate(lobbyButtonPrefab, lobbyListPanel);
        btn.GetComponentInChildren<TMP_Text>().text = "JOIN TO " + lobby.name + " - " + lobby.count;

        btn.GetComponent<Button>().onClick.AddListener(() =>
        {
            MainManager.Instance.lobbyId = lobby.id;
            Debug.Log("Katýlýnýyor: " + lobby.id);
            SceneManager.LoadScene("Level1");
        });
    }
}