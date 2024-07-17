using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ChatData",menuName ="ScriptableObjects/ChatData")]
public class ChatData : ScriptableObject
{
    public List<ChatMessages> chatMessages = new List<ChatMessages>();
}
[System.Serializable]
public class ChatMessages {
    public string
        Name;
    public string
        Messages;
    public int
        msgCount;
    public bool
        isRead;
    public List<string>
         chatSender = new List<string>();
    public List<string>
          chatMsg = new List<string>();
}
