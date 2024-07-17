using System.Collections;
using System.Collections.Generic;
using Photon.Chat.Demo;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextChatInstance : MonoBehaviour
{
    public Text personName;
    public int msgCount;
    public GameObject MsgCountObj;
    public Text prevMsg;
    private string nameOfSender;
    public Color readColor;
    public Color unreadColor;
    public Text InitialName;
    public Image ProfileImage;
    public void SetSenderName()
    {
        PrivateChatMessenger.Instance.targetName = personName.text;
        UIManager.Instance.PrivateChatBox.SetActive(true);
        PrivateChatMessenger.Instance.GetMessage(personName.text);
        msgCount = 0;
        MsgCountObj.SetActive(false);
        prevMsg.GetComponent<Text>().color = readColor;
    }
    public void SetUnreadColor()
    {
        prevMsg.GetComponent<Text>().color = unreadColor;
    }

}
