using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadyPlayer : MonoBehaviourPunCallbacks
{
    public Button readyButton, playButton;
    public TMP_Text readyCountText;
    public PhotonManager photonManager;
    
    [HideInInspector]
    public int readyCount = 0;

    [HideInInspector]
    public bool isMasterClient => PhotonNetwork.IsMasterClient;

    void Start()
    {
        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
        // Initialize UI elements
        readyButton.onClick.AddListener(OnReadyButtonClicked);
        playButton.onClick.AddListener(OnPlayButtonClicked);

        // Disable play button for non-master clients
        playButton.interactable = isMasterClient;
    }

    void OnReadyButtonClicked()
    {
        // Send an RPC to all clients to update the ready count by 1
        photonView.RPC("UpdateReadyCount", RpcTarget.AllBuffered, 1);
    }

    void OnPlayButtonClicked()
    {
        if (isMasterClient && readyCount == PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC("StartGame", RpcTarget.All);
        }
    }

    [PunRPC]
    void UpdateReadyCount(int increment)
    {
        readyCount += increment;
        readyCountText.text = $"Ready Players: {readyCount}";

        // Enable play button if all clients are ready
        if (isMasterClient && readyCount == PhotonNetwork.PlayerList.Length)
        {
            playButton.interactable = true;
            readyButton.interactable = false;
        }

        if(!isMasterClient)
        {
            readyButton.interactable = false;
        }
    }

    [PunRPC]
    void StartGame()
    {
        photonManager.playGamePanel.SetActive(false);
        PhotonNetwork.LoadLevel("Game");
    }

    // trying to reset and re-enable this ready system when a player leaves prev game, creates a new room and other players join it
    // not working
    // public override void OnJoinedRoom()
    // {
    //     readyCount = 0;
    //     readyCountText.text = $"Ready Players: {readyCount}";
    //     playButton.interactable = isMasterClient;
    // }

    // public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    // {
    //     if (isMasterClient && readyCount != PhotonNetwork.PlayerList.Length)
    //     {
    //         playButton.interactable = false;
    //     }
    // }
}
