using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


public class Connect : MonoBehaviourPunCallbacks
{
    public Text displayInfo;
    public GameObject EnterNameScreen;
    public InputField playerName;
    public ServerSettings serverSettings;
    public void SetName()
    {
        if (!string.IsNullOrEmpty(playerName.text))
        {
            PhotonNetwork.NickName = playerName.text;
            EnterNameScreen.SetActive(false);
            StartConnecting();
        }
    }
    private void StartConnecting()
    {
        displayInfo.text = "Connecting......";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1.0";
        serverSettings.DevRegion = "us";
        displayInfo.text = serverSettings.DevRegion;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        displayInfo.text = "Connected";
        PhotonNetwork.JoinLobby();
        displayInfo.text = "Joinning Lobby";
    }
    public override void OnJoinedLobby()
    {
        displayInfo.text = "Joined Lobby";
        JoinRoom();
    }
    private void JoinRoom()
    {
        RoomOptions room = new RoomOptions();
        room.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom("Room", room, TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        displayInfo.text = "Joined Room";
        PhotonNetwork.LoadLevel(1);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        displayInfo.text = "Failed to join random room due to " + message;
        JoinRoom();
    }
    private ExitGames.Client.Photon.Hashtable _myCustumProperties = new ExitGames.Client.Photon.Hashtable();

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {

    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        displayInfo.text = "Failed to join room due to " + message;
        JoinRoom();

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        displayInfo.text = "Room creation Failed due to " + message;
        JoinRoom();
    }
}
