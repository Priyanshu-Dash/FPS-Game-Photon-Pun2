using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Text privateChatNameText;
    public Transform messengerParent;
    public GameObject messengerInstance;
    public GameObject PrivateChatBox;
    public GameObject
        publicRecieverMsg,
        publicSenderMsg,
        publicChatParent,
        recieverMessagePrivatePrefab,
        PrivateChatParentMsgs,
        senderMessagePrivatePrefab,
       PersonalTextChatParent,
        PublicChatEmojiPanel,
        PrivateChatEmojiPanel;
    public Sprite[] Emojis;
    public Image 
         PublicEmojiEffect,
         PrivateEmojiEffect;
    public Color[]
        chatColors;
    public void ScaleUp(Transform transform) {
        StartCoroutine(Scaling(transform));

    }
   IEnumerator Scaling(Transform transform) {
        float i = 1.2f;
        while (i >= 0) {
            yield return new WaitForSeconds(.1f);
            i -= .1f;
            transform.localScale=new Vector3(i,i,i);
        }
    }
}
