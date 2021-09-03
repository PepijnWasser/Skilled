using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public InputField chatInputField;
    public VerticalLayoutGroup chatDisplayContent;

    public GameObject chatMessagePrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(chatInputField.text != "")
            {
                SendChatMessage();
            }
        }
    }

    void SendChatMessage()
    {
        GameObject.FindObjectOfType<LobbyState>().GetComponent<LobbyState>().SendChatMessage(chatInputField.text);
        chatInputField.text = "";
    }

    public void DisplayMessage(ChatMessage message)
    {
        GameObject chatMessage = Instantiate(chatMessagePrefab, chatDisplayContent.transform);

        string timeToDisplay = "[" + message.hourSend + ":" + message.minuteSend + ":" + message.secondSend;
        chatMessage.GetComponent<ChatPrefabManager>().Setmessage(message.sender, timeToDisplay, message.chatMessage);
    }
}
