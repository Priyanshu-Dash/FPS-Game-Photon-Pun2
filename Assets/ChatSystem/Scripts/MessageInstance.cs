using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageInstance : MonoBehaviour
{
    public RectTransform Parent;
    public RectTransform Child;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI senderNameText;
    public float MsgDefaultWidth;
    public float MsgDefaultLength;
    public bool isRecieveMsg;
    public bool isPrivateMsg;
    private bool ismsgLong;
    private float pos;
    void Start()
    {
        StartCoroutine(SetSize());
    }
    IEnumerator SetSize()
    {
        yield return new WaitForSeconds(.5f);
        MessageInstance p = gameObject.GetComponent<MessageInstance>();
       Destroy(p);
    }
    private void Update()
    {
        if (isRecieveMsg && !isPrivateMsg)
        {
            if (messageText.gameObject.GetComponent<TextMeshProUGUI>().text.Length < senderNameText.gameObject.GetComponent<TextMeshProUGUI>().text.Length)
            {
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(senderNameText.GetComponent<RectTransform>().sizeDelta.x + 8, gameObject.GetComponent<RectTransform>().sizeDelta.y);
                gameObject.GetComponent<VerticalLayoutGroup>().padding.left = (int)(MsgDefaultWidth - gameObject.GetComponent<RectTransform>().sizeDelta.x);

            }
            else
            {
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(messageText.GetComponent<RectTransform>().sizeDelta.x + 8, gameObject.GetComponent<RectTransform>().sizeDelta.y);

                gameObject.GetComponent<VerticalLayoutGroup>().padding.left = (int)(MsgDefaultWidth - gameObject.GetComponent<RectTransform>().sizeDelta.x);
            }
        }
        Parent.sizeDelta = new Vector2(Child.GetComponent<RectTransform>().sizeDelta.x + 10, Child.GetComponent<RectTransform>().sizeDelta.y + 8);
        if (isPrivateMsg)
        {
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(messageText.GetComponent<RectTransform>().sizeDelta.x + 10, messageText.GetComponent<RectTransform>().sizeDelta.y + 8);
            gameObject.GetComponent<VerticalLayoutGroup>().padding.left = (int)(MsgDefaultWidth - gameObject.GetComponent<RectTransform>().sizeDelta.x);
        }
        if (messageText.gameObject.GetComponent<TextMeshProUGUI>().text.Length <= MsgDefaultLength)
        {
            messageText.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            if (isPrivateMsg)
            {
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(messageText.GetComponent<RectTransform>().sizeDelta.x + 10, messageText.GetComponent<RectTransform>().sizeDelta.y + 8);
                gameObject.GetComponent<VerticalLayoutGroup>().padding.left = (int)(MsgDefaultWidth - gameObject.GetComponent<RectTransform>().sizeDelta.x);
         
            }
        }
        else
        {
            messageText.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            if (isPrivateMsg)
            {
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(messageText.GetComponent<RectTransform>().sizeDelta.x + 10, messageText.GetComponent<RectTransform>().sizeDelta.y + 8);
                gameObject.GetComponent<VerticalLayoutGroup>().padding.left = (int)(MsgDefaultWidth - gameObject.GetComponent<RectTransform>().sizeDelta.x);
            }
        }
    }

}
