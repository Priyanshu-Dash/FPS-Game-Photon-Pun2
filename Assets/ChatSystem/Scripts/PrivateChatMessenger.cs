//using Photon.Pun;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class PrivateChatManager :Singleton<PrivateChatManager>
//{
//    public InputField inputField;
//   // public string targetName;
//    public Text displayChat;
//    public ChatData chatData;
//    public ChatMessage chatMessage;
//    private PhotonView photonView;
//    private bool isSender,isFound;
//    void Start()
//    {
//        photonView=GetComponent<PhotonView>();
//    }
//    //On Press Enter Send Message
//    void Update() {
//        if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) {
//            SendPrivateMessage();
//        }
//    }
//    //Send a Rpc for Private Message
//    public void SendPrivateMessage() {
//        //Only send a message when user has type something.
//        if (!string.IsNullOrEmpty(inputField.text)&& !string.IsNullOrEmpty(UIManager.Instance.privateChatNameText.text)) {
//            isSender = true;//Only true on the side of sender
//            photonView.RPC("MessageSent", RpcTarget.All, PhotonNetwork.NickName, UIManager.Instance.privateChatNameText.text, inputField.text);
//            inputField.text = "";//Once Message sent it clear from our text field.
//            inputField.ActivateInputField();
//        }
//    }
//    [PunRPC]
//    //sender ID is the name of sender
//    //reciever Id is name of who's gonna recieve the message.
//    //Message is string that sender will send to dedicated reciever.
//    private void MessageSent(string senderId,string receiverId,string Message) {
//        displayChat.text = "";
//        //RPC execute only on sender or reciever, not any other user
//        if (PhotonNetwork.NickName == senderId || PhotonNetwork.NickName == receiverId) {
//            string chatName;
//            //On the side of sender it search with reciever name
//            if (isSender) { chatName = receiverId; } 
//            //On reciever side it will search with sender name
//            else { chatName = senderId; }
//            //Iterating if in CHatData we already have user name
//            for(int i = 0; i < chatData.chatMessages.Count; i++) {
//                if (chatName == chatData.chatMessages[i].name){
//                    chatData.chatMessages[i].message += "\n" + "<b><color=red>" + senderId + "</color></b>" + ": " + Message;
//                  if(UIManager.Instance.privateChatNameText.text==chatName)  displayChat.text=chatData.chatMessages[i].message;//Display Message on Screen.
//                    isFound= true;
//                }
//            }
//            //If You are chatting to a user for first time
//            if (!isFound) {
//                ChatMessage chat = new ChatMessage();//new chat messge object for stroirng new user name and message
//                chatData.chatMessages.Add(chat);//adding it to opur list of whole chatData.
//                chatData.chatMessages[chatData.chatMessages.Count-1].name= chatName;//In new instancte first save name of our sender by making index 1 less as it start from zero.
//                chatData.chatMessages[chatData.chatMessages.Count-1]. message= "\n"+ "<b><color=red>"+senderId + "</color></b>" + ": " +Message;//Saving Message
//                if (UIManager.Instance.privateChatNameText.text == chatName) displayChat.text = chatData.chatMessages[chatData.chatMessages.Count - 1].message;//DisplayMessage on Screen
//            }
//            isFound= false;//Making these bool false for next RPC.
//            isSender = false;
//        }
//    }
//    //Retrieve Previous Messages
//    public void GetChatMessages(string name) {
//        displayChat.text = "";
//        //If userName is there it means you have previous Chat
//        for (int i = 0; i < chatData.chatMessages.Count; i++) {
//            if (name == chatData.chatMessages[i].name) {
//                displayChat.text = chatData.chatMessages[i].message;
//            }
//        }
//    }
//    public void SendEmojiEffect(int index) {
//        photonView.RPC("ShowEmojiPrivate", RpcTarget.All, index,PhotonNetwork.NickName, UIManager.Instance.privateChatNameText.text);
//        UIManager.Instance.PrivateChatEmojiPanel.SetActive(false);//Only diable on player side sending rpc to other players
//    }
//    [PunRPC]
//    private void ShowEmojiPrivate(int index, string senderId, string receiverId) {
//        if ((PhotonNetwork.NickName == senderId || PhotonNetwork.NickName == receiverId)&&(UIManager.Instance.privateChatNameText.text==senderId|| UIManager.Instance.privateChatNameText.text ==receiverId)) {
//            UIManager.Instance.PrivateEmojiEffect.gameObject.SetActive(true);//first activate it
//            UIManager.Instance.PrivateEmojiEffect.GetComponent<Image>().sprite = UIManager.Instance.Emojis[index];//then place the effect on image
//            UIManager.Instance.ScaleUp(UIManager.Instance.PrivateEmojiEffect.GetComponent<Transform>().transform);
//            StartCoroutine(DisableEmojiEffect());//after sometime disable it.
//        }
//    }
//    IEnumerator DisableEmojiEffect() {
//        yield return new WaitForSeconds(3);
//        UIManager.Instance.PrivateEmojiEffect.gameObject.SetActive(false);
//    }
//    public void OpenEmojiPanel() {
//        UIManager.Instance.PrivateChatEmojiPanel.SetActive(true);
//    }
//}
using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
//using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrivateChatMessenger : MonoBehaviourPunCallbacks {
    public GameObject privateInputField;
    public GameObject ChatPanel;
    public Text SelectedChannelText;
    public Text PersonName;
    public string targetName;
    public ChatData chatData;
    public ChatMessages chatMessages;
    private bool IsFOund, isSender;
    public static PrivateChatMessenger Instance;
    private GameObject msgs = null;
    private string chatMsgDummy = "", senderNameDummy;
    private int lengthData;
    private bool isUserthere;
    private bool isfoundMsg;
    private int userIndex, dumyNum;
    private string notifyName;
    private string msgreturn;
    void Awake() {
        if (Instance) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        chatData.chatMessages.Clear();
    }
    private void Update() {
        OnEnterSend();
    }
    public void OnEnterSend() {
        if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) {
            SendPrivateMessage();
        }
    }
    public void TypeValueChanged(string valueIn) {
        //  Debug.Log("typing");
        RemoveMsgCount();
    }
    public void SendPrivateMessage() {
        if (!string.IsNullOrEmpty(privateInputField.GetComponent<InputField>().text) && !string.IsNullOrWhiteSpace(privateInputField.GetComponent<InputField>().text) && !string.IsNullOrEmpty(targetName) && !string.IsNullOrEmpty(PhotonNetwork.NickName)) {
            isSender = true;
            photonView.RPC("CheckChatMessage", RpcTarget.All, PhotonNetwork.NickName, targetName, privateInputField.GetComponent<InputField>().text);
            privateInputField.GetComponent<InputField>().text = "";
            privateInputField.GetComponent<InputField>().ActivateInputField();
        }
    }

    [PunRPC]
    private void CheckChatMessage(string sender, string reciever, string Message) {
        if (PhotonNetwork.NickName == sender || PhotonNetwork.NickName == reciever)//only two side
        {
            string chatName;
            if (isSender) { chatName = reciever; }//For sender chat searching name will be with reciever
            else { chatName = sender; }//For reciever will search with the name of sender
            #region SavingData
            for (int i = 0; i < chatData.chatMessages.Count; i++) {
                if (chatData.chatMessages[i].Name == chatName) {
                    chatData.chatMessages[i].chatSender.Add(sender);
                    chatData.chatMessages[i].chatMsg.Add(Message);
                    //chatData.chatMessages[i].Messages += "\n" + sender + "+" + Message;
                    senderNameDummy = sender;
                    chatMsgDummy = Message;
                    IsFOund = true;
                }
            }
            if (!IsFOund) {
                lengthData = chatData.chatMessages.Count;
                //if (lengthData > 0) { lengthData -= 1; }
                //  else { lengthData = 0; }
                Debug.Log("length" + lengthData);
                ChatMessages chat = new ChatMessages();
                chatData.chatMessages.Add(chat);
                chatData.chatMessages[lengthData].chatSender.Add(sender);
                chatData.chatMessages[lengthData].chatMsg.Add(Message);
                senderNameDummy = sender;
                chatMsgDummy = Message;
                chatData.chatMessages[lengthData].Name = chatName;
                // chatData.chatMessages[lengthData].Messages += "\n" + sender + "+" + Message;
                //  if (PersonName.text == chatName) SelectedChannelText.text = chatData.chatMessages[chatData.chatMessages.Count - 1].Messages;
            }
            #endregion SavingData
            if (isSender) { ShowMessageInInstance(reciever, Message); } else { ShowMessageInInstance(sender, Message); }
            #region DisplayMsg
            if (PersonName.text == chatName) {
                if (isSender) {
                    msgs = Instantiate(UIManager.Instance.recieverMessagePrivatePrefab, UIManager.Instance.PrivateChatParentMsgs.transform);
                } else {
                    msgs = Instantiate(UIManager.Instance.senderMessagePrivatePrefab, UIManager.Instance.PrivateChatParentMsgs.transform);
                }
                // msgs.GetComponent<MessageInstance>().senderNameText.text = senderNameDummy;
                msgs.GetComponent<MessageInstance>().messageText.text = chatMsgDummy;
            }
            #endregion DisplayMsg
            IsFOund = false;
            if (!isSender) { StartCoroutine(NotificationOfMessage(chatName)); }//Only display Messgae on recieving side.
            isSender = false;
        }
    }
    public string ShowLatestMSgInText(string name) {
        Debug.Log("nameeeeeeeeeeeeeee" + name);

        for (int i = 0; i < chatData.chatMessages.Count; i++) {
            if (chatData.chatMessages[i].Name == name) {
                isfoundMsg = true;
                dumyNum = i;
                break;
                // SelectedChannelText.text = chatData.chatMessages[i].Messages;
            }

        }
        msgreturn = "";
        if (isfoundMsg) {
            Debug.Log(chatData.chatMessages[dumyNum].chatMsg[chatData.chatMessages[dumyNum].chatMsg.Count - 1] + "Messsssssssssssage+");
            isfoundMsg = false;
            msgreturn = chatData.chatMessages[dumyNum].chatMsg[chatData.chatMessages[dumyNum].chatMsg.Count - 1];
        }
        return msgreturn;
    }
    public void GetMessage(string name) {
        PersonName.text = name;
        SelectedChannelText.text = "";
        int num = 0;

        for (int i = 0; i < chatData.chatMessages.Count; i++) {
            if (chatData.chatMessages[i].Name == name) {
                num = i;
                SelectedChannelText.text = chatData.chatMessages[i].Messages;
            }
        }
        DestroyPrevMsg();
        if (chatData.chatMessages.Count > 0) {
            GetPreviousMsgs(num, name);
        }
    }
    private void DestroyPrevMsg() {
        for (int i = 0; i < UIManager.Instance.PrivateChatParentMsgs.transform.childCount; i++) {
            Destroy(UIManager.Instance.PrivateChatParentMsgs.transform.GetChild(i).gameObject);
        }
    }
    private void GetPreviousMsgs(int index, string nam) {
        for (int i = 0; i < chatData.chatMessages.Count; i++) {
            if (nam == chatData.chatMessages[i].Name) {
                isUserthere = true;
                userIndex = i;
                Debug.Log(i + "User name is" + nam);
                break;
            }
        }
        if (isUserthere) {
            for (int i = 0; i < chatData.chatMessages[userIndex].chatSender.Count; i++) {
                if (PhotonNetwork.NickName == chatData.chatMessages[userIndex].chatSender[i]) {
                    msgs = Instantiate(UIManager.Instance.recieverMessagePrivatePrefab, UIManager.Instance.PrivateChatParentMsgs.transform);
                } else {
                    msgs = Instantiate(UIManager.Instance.senderMessagePrivatePrefab, UIManager.Instance.PrivateChatParentMsgs.transform);
                }
                msgs.GetComponent<MessageInstance>().senderNameText.text = chatData.chatMessages[index].chatSender[i];
                msgs.GetComponent<MessageInstance>().messageText.text = chatData.chatMessages[index].chatMsg[i];
                Debug.Log(chatData.chatMessages[index].chatSender[i]);
                Debug.Log(chatData.chatMessages[index].chatMsg[i]);
            }
        }
        isUserthere = false;

    }
    private void ShowMessageInInstance(string nam, string msg) {
        for (int i = 0; i < UIManager.Instance.PersonalTextChatParent.transform.childCount; i++) {
            if (nam == UIManager.Instance.PersonalTextChatParent.transform.GetChild(i).GetComponent<TextChatInstance>().personName.text) {
                UIManager.Instance.PersonalTextChatParent.transform.GetChild(i).GetComponent<TextChatInstance>().prevMsg.text = msg;
                break;
            }
        }
    }
    IEnumerator NotificationOfMessage(string name) {
        notifyName = name;
        UnreadMsgCOunt(name);
        string[] myname = name.Split('(');
        // UIManager.Instance.MessageNotify.text = myname[1].Replace(")", "") + " sent a message";
        //  UIManager.Instance.MessageNotify.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        // UIManager.Instance.MessageNotify.text = "";
        // UIManager.Instance.MessageNotify.transform.parent.gameObject.SetActive(false);
    }
    private void UnreadMsgCOunt(string name) {
        for (int i = 0; i < UIManager.Instance.PersonalTextChatParent.transform.childCount; i++) {
            if (UIManager.Instance.PersonalTextChatParent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text == name) {
                UIManager.Instance.PersonalTextChatParent.transform.GetChild(i).GetComponent<TextChatInstance>().msgCount += 1;
                UIManager.Instance.PersonalTextChatParent.transform.GetChild(i).GetComponent<TextChatInstance>().MsgCountObj.SetActive(true);
                UIManager.Instance.PersonalTextChatParent.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text = UIManager.Instance.PersonalTextChatParent.transform.GetChild(i).GetComponent<TextChatInstance>().msgCount.ToString();
                UIManager.Instance.PersonalTextChatParent.transform.GetChild(i).GetComponent<TextChatInstance>().SetUnreadColor();
            }
        }
    }
    private void RemoveMsgCount() {
        for (int i = 0; i < UIManager.Instance.PersonalTextChatParent.transform.childCount; i++) {
            if (UIManager.Instance.PersonalTextChatParent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text == PersonName.text) {
                UIManager.Instance.PersonalTextChatParent.transform.GetChild(i).GetComponent<TextChatInstance>().MsgCountObj.SetActive(false);
                UIManager.Instance.PersonalTextChatParent.transform.GetChild(i).GetComponent<TextChatInstance>().msgCount = 0;

            }
        }
    }
    public void CloseChatPanel(bool b) {
        ChatPanel.SetActive(b);
    }
    public void OpenChatPanel() {
        GetMessage(notifyName);
        RemoveMsgCount();
        ChatPanel.SetActive(true);
    }
    public void SendEmojiEffect(int index) {
        photonView.RPC("ShowEmojiPrivate", RpcTarget.All, index, PhotonNetwork.NickName, UIManager.Instance.privateChatNameText.text);
        UIManager.Instance.PrivateChatEmojiPanel.SetActive(false);//Only diable on player side sending rpc to other players
    }
    [PunRPC]
    private void ShowEmojiPrivate(int index, string senderId, string receiverId) {
        if ((PhotonNetwork.NickName == senderId || PhotonNetwork.NickName == receiverId) && (UIManager.Instance.privateChatNameText.text == senderId || UIManager.Instance.privateChatNameText.text == receiverId)) {
            UIManager.Instance.PrivateEmojiEffect.gameObject.SetActive(true);//first activate it
            UIManager.Instance.PrivateEmojiEffect.GetComponent<Image>().sprite = UIManager.Instance.Emojis[index];//then place the effect on image
            UIManager.Instance.ScaleUp(UIManager.Instance.PrivateEmojiEffect.GetComponent<Transform>().transform);
            StartCoroutine(DisableEmojiEffect());//after sometime disable it.
        }
    }
    IEnumerator DisableEmojiEffect() {
        yield return new WaitForSeconds(3);
        UIManager.Instance.PrivateEmojiEffect.gameObject.SetActive(false);
    }
    public void OpenEmojiPanel() {
        UIManager.Instance.PrivateChatEmojiPanel.SetActive(true);
    }
}
