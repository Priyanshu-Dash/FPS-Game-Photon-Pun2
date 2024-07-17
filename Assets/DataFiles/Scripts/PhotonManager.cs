using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    #region Public_Variables
    public TMP_InputField playerName, newRoomName, maxPlayers, roomToJoin;
    public GameObject loginPanel, connectingPanel, lobbyPanel, createRoomPanel, playGamePanel, joinRoomPanel;
    public GameObject playerListContent, playDetails;
    public GameObject playButton;
    public TMP_Text roomName;
    public string GameSceneName;
    #endregion

    #region Private_Variables
    private Dictionary<int, GameObject> playerListEntries;

    #endregion

    #region UnityMethods
    void Start()
    {
        playButton.GetComponent<Button>().onClick.AddListener(OnPlayClicked);

        if (PhotonNetwork.IsConnected)
        {
            ActivatePanel(lobbyPanel.name);
        }
        else
        {
            ActivatePanel(loginPanel.name);
        }
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Update()
    {
        Debug.Log("Network State: " + PhotonNetwork.NetworkClientState);
    }

    #endregion

    #region UIMethods
    public void OnLoginClick()
    {
        string name = playerName.text + "_" + System.Guid.NewGuid().ToString("N")[..4];

        if (!string.IsNullOrEmpty(name))
        {
            PhotonNetwork.LocalPlayer.NickName = name;
            PhotonNetwork.ConnectUsingSettings();
            ActivatePanel(connectingPanel.name);
        }
        else
        {
            Debug.Log("Player name is invalid");
        }
    }

    public void OnLogoutClick()
    {
        PhotonNetwork.Disconnect();
        ActivatePanel(loginPanel.name);
    }

    public void OnLeaveRoomClick()
    {
        PhotonNetwork.LeaveRoom();
        ActivatePanel(lobbyPanel.name);
    }

    public void OnCreateRoomClick()
    {
        string room = newRoomName.text;

        if (maxPlayers.text == "")
        {
            maxPlayers.text = "4";
        }

        if (!string.IsNullOrEmpty(room))
        {
            RoomOptions roomOptions = new()
            {
                MaxPlayers = byte.Parse(maxPlayers.text),
                IsVisible = true,  // Make sure the room is visible
                IsOpen = true      // Make sure the room is open
            };
            PhotonNetwork.CreateRoom(room, roomOptions);
        }

        else
        {
            RoomOptions roomOptions = new()
            {
                IsVisible = true,  // Make sure the room is visible
                IsOpen = true      // Make sure the room is open
            };
            PhotonNetwork.CreateRoom(null, roomOptions);
        }
    }

    public void OnJoinRoomClick()
    {
        string room = roomToJoin.text;

        if (!string.IsNullOrEmpty(room))
        {
            PhotonNetwork.JoinRoom(room);
        }
        else
        {
            Debug.Log("Room name is invalid");
        }
    }

    #endregion

    #region Utility Methods
    public void ActivatePanel(string panelName)
    {
        loginPanel.SetActive(panelName.Equals(loginPanel.name));
        connectingPanel.SetActive(panelName.Equals(connectingPanel.name));
        lobbyPanel.SetActive(panelName.Equals(lobbyPanel.name));
        joinRoomPanel.SetActive(panelName.Equals(joinRoomPanel.name));
        createRoomPanel.SetActive(panelName.Equals(createRoomPanel.name));
        playGamePanel.SetActive(panelName.Equals(playGamePanel.name));
    }

    private void AddPlayerToList(Player player)
    {
        if (!playerListEntries.ContainsKey(player.ActorNumber))
        {
            GameObject playerObj = Instantiate(playDetails, playerListContent.transform);
            playerObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = player.NickName;

            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                playerObj.transform.GetChild(1).gameObject.SetActive(true);
            }

            playerListEntries.Add(player.ActorNumber, playerObj);
        }
    }

    void OnPlayClicked()
    {
        PhotonNetwork.LoadLevel(GameSceneName);
    }
    #endregion

    #region Photon_Callbacks
    public override void OnConnected()
    {
        Debug.Log("Connected to internet");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " disconnected from Photon: " + cause.ToString());
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " connected to Photon!");
        ActivatePanel(lobbyPanel.name);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room " + PhotonNetwork.CurrentRoom.Name + " created successfully!");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined room " + PhotonNetwork.CurrentRoom.Name + " successfully!");

        ActivatePanel(playGamePanel.name);
        roomName.text = "Room: " + PhotonNetwork.CurrentRoom.Name;

        if (PhotonNetwork.IsMasterClient)
        {
            playButton.SetActive(true);
        }

        playerListEntries ??= new Dictionary<int, GameObject>();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            AddPlayerToList(p);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerToList(newPlayer);
    }

    // Called for REMOTE PLAYER (if any player other than me leaves)
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);

        if (PhotonNetwork.IsMasterClient)
        {
            playButton.SetActive(true);
        }
    }

    // Called for LOCAL PLAYER (if I leave)
    /// <summary>
    /// Local player is me. For me eveyr other player is remote player.
    /// So, when I leave, I need to destroy all the remote players on my game
    /// (this does not destroy all players from every player's game)
    /// </summary>
    public override void OnLeftRoom()
    {
        foreach (GameObject player in playerListEntries.Values)
        {
            Destroy(player.gameObject);
        }
        playerListEntries.Clear();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
        switch (returnCode)
        {
            case ErrorCode.GameDoesNotExist:
                Debug.LogError("The room does not exist. Please make sure you have the correct room name.");
                break;
            case ErrorCode.GameFull:
                Debug.LogError("The room is full.");
                break;
            case ErrorCode.GameClosed:
                Debug.LogError("The room is closed.");
                break;
            default:
                Debug.LogError("An unknown error occurred. Error code: " + returnCode);
                break;
        }
        // Re-activate the Lobby Panel to allow the user to try again
        ActivatePanel(lobbyPanel.name);
    }

    #endregion
}
