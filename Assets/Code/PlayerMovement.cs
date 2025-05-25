using UnityEngine;
using UnityEngine.InputSystem;
using NativeWebSocket;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;

public class PlayerMovement : MonoBehaviour
{
    WebSocket websocket;
    public int wsMyId;
    static List<WSPlayer> wsPlayers;
    public static string JwtKey;

    [Header("Slave Player")]
    public GameObject slavePlayerPrefab;
    private Dictionary<int, GameObject> slavePlayers = new Dictionary<int, GameObject>();

    public AudioClip backgroundMusic;
    public Rigidbody2D rb;
    public Animator ani;

    [Header("Movement")]
    public float moveSpeed = 5.0f;
    Vector2 movement;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float fallSpeedMultiplier = 2f;
    public float maxFallSpeed = 20f;

    [Header("Jump")]
    public float jumpPower = 10.0f;
    public int maxJump = 2;
    public int jumpRemaining = 0;
    public float jumpBufferTime = 0.15f;
    float jumpBuffer;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    [Header("Sound")]
    public AudioClip jumpPlayer;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer == null)
            Debug.LogWarning("SpriteRenderer not found on PlayerMovement!");
        GetTheKey();
        WebSocketStart();
    }

    private void GetTheKey()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "userkey.txt");
        Debug.Log("Save File Path: " + filePath);
        if (File.Exists(filePath))
        {
            JwtKey = File.ReadAllText(filePath);
        }
        else
        {
            JwtKey = null;
            Debug.Log("NO KEY!!!!!!!!");
        }
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if(websocket != null)
            websocket.DispatchMessageQueue();
#endif
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
        GroundedCheck();
        Gravity();
        UpdateAnimation();
        FlipCharacter();
        SendUpdatesToWS();
        UpdateSlavePlayerPositions();
    }

    private void SendUpdatesToWS()
    {
        if (wsPlayers != null)
        {
            Gizmos.color = Color.yellow;
            var meP = wsPlayers.Find(p => p.id == wsMyId);

            if (meP != null && meP.pos != null)
            {
                if(meP.pos.x != transform.position.x || meP.pos.y != transform.position.y)
                {
                    string joinJOSN = "{\"type\": \"uPos\",\"pos\": {\"x\":" + transform.position.x.ToString(CultureInfo.InvariantCulture) + ",\"y\":"+ transform.position.y.ToString(CultureInfo.InvariantCulture) + "}}";

                    websocket.SendText(joinJOSN);
                    meP.pos.x = transform.position.x;
                    meP.pos.y = transform.position.y;
                }
            }
            else
            {
                Debug.LogWarning("Player or Player position null in wsPlayers.");
            }
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        movement.x = ctx.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (jumpRemaining <= 0)
            {
                jumpBuffer = Time.fixedTime;
                return;
            }
            PerformJump();
        }
        else if (ctx.canceled)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    public void PerformJump()
    {
        AudioSource.PlayClipAtPoint(jumpPlayer, transform.position);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        jumpRemaining--;
        ani.SetTrigger("Jump");
    }

    public void Bounce(float force)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
    }

    public void GroundedCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer) &&
            rb.linearVelocity.y <= 0.05f)
        {
            jumpRemaining = maxJump;
            if (jumpBuffer + jumpBufferTime >= Time.fixedTime)
            {
                PerformJump();
            }
        }
    }

    public void Gravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void UpdateAnimation()
    {
        ani.SetFloat("Magnitude", rb.linearVelocity.magnitude);
        ani.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void FlipCharacter()
    {
        if (movement.x > 0)
            spriteRenderer.flipX = false;
        else if (movement.x < 0)
            spriteRenderer.flipX = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }


    // ---------- WebSocket Part
    async void WebSocketStart()
    {
        //websocket = new WebSocket("ws://localhost:8080");
        websocket = new WebSocket("wss://aaa.evrenomi.com", new() {
            {"Authorization", $"Bearer {JwtKey}"}
        });

        websocket.OnOpen += () =>
        {
            string joinJOSN = "{\"type\": \"join\",\"lobbyId\": "+ MainManager.Instance.lobbyId + "}";

            websocket.SendText(joinJOSN);
            Debug.Log("Connection open!\n"+joinJOSN);
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
            Debug.Log(e);
            DestroyAllSlavePlayers();
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("OnMessage! " + message);
            WSType wst = JsonUtility.FromJson<WSType>(message);
            
            switch (wst.type)
            {
                case "err":
                    Debug.Log("WS = ERR: " + wst.detail);
                    break;
                case "conf":
                    // {"type":"conf","what":"join","yourPlayerId":1,"players":[{"id":1,"pos":{"x":0,"y":0},"coins":0}]}
                    Debug.Log("WS = CONF: " + wst.what);
                    if(wst.what == "join")
                    {
                        Debug.Log("WS - CONF MyId: " + wst.yourPlayerId);
                        wsMyId = wst.yourPlayerId;
                        wsPlayers = wst.players.ToList();

                        CreateSlavePlayers();
                    }
                    break;
                case "event":
                    switch (wst.@event)
                    {
                        case "newPlayer":
                            // {"type":"event","event":"newPlayer","newPlayer":{"id":2,"pos":{"x":0,"y":0},"coins":0}}
                            Debug.Log("WS - EVENT newPlayer id: " + wst.newPlayer.id);
                            wsPlayers.Add(wst.newPlayer);
                            
                            CreateOrUpdateSlavePlayer(wst.newPlayer);
                            break;
                        case "kickPlayer":
                            // {"type":"event","event":"kickPlayer","playerId":2}
                            Debug.Log("WS - EVENT kickPlayer id: " + wst.playerId);
                            wsPlayers.RemoveAll(p => p.id == wst.playerId);

                            DestroySlavePlayer(wst.playerId);
                            break;
                        case "uPos":
                            // {"type":"event","event":"uPos","playerId":3,"pos":{"x":4,"y":2}}
                            Debug.Log("WS - EVENT uPos id: " + wst.playerId);
                            WSPlayer theP = wsPlayers.Find(p => p.id == wst.playerId);
                            theP.pos.x = wst.pos.x;
                            theP.pos.y = wst.pos.y;

                            UpdateSlavePlayerPosition(wst.playerId, wst.pos);
                            break;
                        default:
                            Debug.LogWarning("WS = UNKNOWN EVENT: " + wst.@event);
                            break;
                    }
                    break;
                default:
                    Debug.LogWarning("Unknow ws type: " + wst.type);
                    break;
            }
        };

        // Keep sending messages at every 0.3s
        //InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        // waiting for messages
        await websocket.Connect();

        //MainManager.Instance.lobbyId;
    }

    void OnDrawGizmos()
    {
        if (wsPlayers != null)
        {
            Gizmos.color = Color.yellow;
            foreach (var player in wsPlayers)
            {
                if (player != null && player.pos != null)
                {
                    Vector3 playerPosition = new Vector3(player.pos.x, player.pos.y, 0); // Assuming Z is 0
                    Gizmos.DrawWireCube(playerPosition, Vector3.one); // Draw a 1x1x1 cube
                }
                else
                {
                    Debug.LogWarning("Player or Player position null in wsPlayers.");
                }
            }
        }
    }
    //--------------- Slave Player Management
    void CreateSlavePlayers()
    {
        if (wsPlayers == null) return;

        foreach (var player in wsPlayers)
        {
            if (player.id != wsMyId) // Don't create a slave for the local player
            {
                CreateOrUpdateSlavePlayer(player);
            }
        }
    }

    void CreateOrUpdateSlavePlayer(WSPlayer player)
    {
        if (player == null || player.pos == null) return;

        if (!slavePlayers.ContainsKey(player.id))
        {
            //Create
            GameObject slave = Instantiate(slavePlayerPrefab);
            slavePlayers.Add(player.id, slave);
        }

        //Update Position
        UpdateSlavePlayerPosition(player.id, player.pos);
    }

    void UpdateSlavePlayerPositions()
    {
        if (wsPlayers == null)
            return;
        foreach (var player in wsPlayers)
        {
            if (player.id != wsMyId) // Don't update local player
            {
                UpdateSlavePlayerPosition(player.id, player.pos);
            }
        }
    }

    void UpdateSlavePlayerPosition(int playerId, WSPos pos)
    {
        if (pos == null) return;

        if (slavePlayers.ContainsKey(playerId))
        {
            GameObject slave = slavePlayers[playerId];
            if (slave != null)
            {
                slave.transform.position = new Vector3(pos.x, pos.y, 0);
            }
            else
            {
                Debug.LogWarning(
                    "Slave player GameObject is null but still in the dictionary"
                );
                slavePlayers.Remove(playerId); // Clean up if necessary
            }
        }
    }

    void DestroySlavePlayer(int playerId)
    {
        if (slavePlayers.ContainsKey(playerId))
        {
            GameObject slave = slavePlayers[playerId];
            if (slave != null)
            {
                Destroy(slave);
            }

            slavePlayers.Remove(playerId);
        }
    }

    void DestroyAllSlavePlayers()
    {
        foreach (var slave in slavePlayers.Values)
        {
            if (slave != null)
            {
                Destroy(slave);
            }
        }

        slavePlayers.Clear();
    }
    [Serializable]
    class WSType
    {
        public string type;
        public string detail;
        public string what;
        public string @event;
        public int playerId;
        public int yourPlayerId;
        public WSPlayer[] players;
        public WSPos pos;
        public WSPlayer newPlayer;
    }
    [Serializable]
    class WSPlayer
    {
        public int id;
        public WSPos pos;
        public int coins;
    }
    [Serializable]
    class WSPos
    {
        public float x;
        public float y;
    }
}
