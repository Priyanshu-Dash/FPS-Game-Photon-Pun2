using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PublicChatManager : MonoBehaviour, IChatClientListener {
    public string[] channels = new string[1];
    public InputField InputField;
    public Text displayChat;
    ChatClient chatClient;
    PhotonView photonView;
    void Start() {
        photonView=GetComponent<PhotonView>();
        SetChatSettings();
    }
    void Update() {
        if (this.chatClient != null) {
            this.chatClient.Service();
        }
        if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) {
            SendPublicMessage();
        }
    }
    public void SetChatSettings() {
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(PhotonNetwork.NickName));
        Debug.Log("Conecting");
    }
    public void SendPublicMessage() {
        if (!string.IsNullOrEmpty(InputField.text)) {
            chatClient.PublishMessage(channels[0], InputField.text);
            InputField.text = "";
            InputField.ActivateInputField();
        }
    }
    public void SendEmojiEffect(int index) {
        photonView.RPC("ShowEmoji", RpcTarget.All,index);
        UIManager.Instance.PublicChatEmojiPanel.SetActive(false);//Only diable on player side sending rpc to other players
    }
    [PunRPC]
    private void ShowEmoji(int index) {
        UIManager.Instance.PublicEmojiEffect.gameObject.SetActive(true);//first activate it
        UIManager.Instance.PublicEmojiEffect.GetComponent<Image>().sprite = UIManager.Instance.Emojis[index];//then place the effect on image
        UIManager.Instance.ScaleUp(UIManager.Instance.PublicEmojiEffect.GetComponent<Transform>().transform);
        StartCoroutine(DisableEmojiEffect());//after sometime disable it.
    }
    IEnumerator DisableEmojiEffect() {
        yield return new WaitForSeconds(3);
        UIManager.Instance.PublicEmojiEffect.gameObject.SetActive(false);
    }
    public void OpenEmojiPanel() {
        UIManager.Instance.PublicChatEmojiPanel.SetActive(true);
    }
    #region callBack
    public void DebugReturn(DebugLevel level, string message) {
    }

    public void OnDisconnected() {
    }

    public void OnConnected() {
        chatClient.Subscribe(channels[0]);
        Debug.Log("connecting to chanel");

    }

    public void OnChatStateChange(ChatState state) {
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages) {
        for(int i = 0; i < senders.Length; i++) {
            displayChat.text+= "\n" + "<b><color=red>"+senders[i]+"</color></b>" + ": " + messages[i];
            if (senders[i] == PhotonNetwork.NickName/*istyu*/) {

                GameObject msgs = Instantiate(UIManager.Instance.publicRecieverMsg, UIManager.Instance.publicChatParent.transform);
                msgs.GetComponent<MessageInstance>().senderNameText.text = senders[i];
                msgs.GetComponent<MessageInstance>().messageText.text = messages[i].ToString();
             //   UIManager.Instance.globalMsg.text = messages[i].ToString();

            } else {
                GameObject msgs = Instantiate(UIManager.Instance.publicSenderMsg, UIManager.Instance.publicChatParent.transform);
                msgs.GetComponent<MessageInstance>().senderNameText.text = senders[i];
               // UIManager.Instance.globalMsg.text = messages[i].ToString();
                msgs.GetComponent<MessageInstance>().messageText.text = messages[i].ToString();
            }
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName) {
    }

    public void OnSubscribed(string[] channels, bool[] results) {
        Debug.Log("joined chanel");
    }

    public void OnUnsubscribed(string[] channels) {
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) {
    }

    public void OnUserSubscribed(string channel, string user) {
    }

    public void OnUserUnsubscribed(string channel, string user) {
    }
    #endregion callback
}