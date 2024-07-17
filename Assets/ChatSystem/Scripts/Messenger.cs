using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Messenger : MonoBehaviourPunCallbacks
{
    private PhotonView photonView;
    private string intialsName;
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        photonView.RPC("ShowPlayerNameInMessenger", RpcTarget.All);
    }
    [PunRPC]
    private void ShowPlayerNameInMessenger()
    {
        //Destroy all players 
        for (int i = 0; i < UIManager.Instance.messengerParent.childCount; i++)
        {
            Destroy(UIManager.Instance.messengerParent.GetChild(i).gameObject);
        }
        //Instantiate Players
        Player[] Player = PhotonNetwork.PlayerList;
        foreach (Player p in Player)
        {
            if (p.NickName != PhotonNetwork.NickName)
            {
                GameObject player = Instantiate(UIManager.Instance.messengerInstance, UIManager.Instance.messengerParent);
                player.transform.GetChild(0).GetComponent<Text>().text = p.NickName;
                intialsName = ReturnFirstName(p.NickName) + ReturnLastName(p.NickName);
                player.GetComponent<TextChatInstance>().InitialName.text = intialsName.ToUpper();
                player.GetComponent<TextChatInstance>().ProfileImage.color = UIManager.Instance.chatColors[Random.Range(0, UIManager.Instance.chatColors.Length)];
            }
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        photonView.RPC("ShowPlayerNameInMessenger", RpcTarget.All);
    }
    #region StringFunctions
    public string ReturnFirstName(string Name)
    {
        return Name[0].ToString();
    }
    public string ReturnLastName(string Name)
    {
        string last = "";
        for (int i = 0; i < Name.Length; i++)
        {
            if (Name[i] == ' ')
            {
                //space found
                last = Name[++i].ToString();
                break;
            }
        }
        return last;
    }
    #endregion StringFunction
}
