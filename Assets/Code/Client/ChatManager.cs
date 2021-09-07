using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChatManager : MonoBehaviour
{
    public InputField chatInputField;
    public VerticalLayoutGroup chatDisplayContent;

    public GameObject chatMessagePrefab;

    LobbyState lobbyState;

    private void Start()
    {
        lobbyState = GameObject.FindObjectOfType<LobbyState>().GetComponent<LobbyState>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (chatInputField.text != "")
            {
                HandleTypedMessage();
            }
        }
    }

    void HandleTypedMessage()
    {
        string inputText = chatInputField.text;

        if (inputText[0].ToString() != "/")
        {
            lobbyState.SendChatMessage(inputText);
        }
        else
        {
            string[] partsOfString = inputText.Split(' ');
            if (partsOfString[0] == "/setname")
            {
                if(partsOfString.Length > 1)
                {
                    if (partsOfString[1] != "")
                    {
                        lobbyState.SendPlayerNameRequest(partsOfString[1]);
                    }
                    else
                    {
                        DisplayMessage("please put in a valid name");
                    }
                }
                else
                {
                    DisplayMessage("please put in a valid name");
                }

            }
            else if (partsOfString[0] == "/help")
            {
                lobbyState.SendHelpRequest();
            }
        }
        chatInputField.text = "";
    }

    public void DisplayMessage(ChatMessage message)
    {
        GameObject chatMessage = Instantiate(chatMessagePrefab, chatDisplayContent.transform);

        string timeToDisplay = "[" + message.hourSend + ":" + message.minuteSend + ":" + message.secondSend;
        chatMessage.GetComponent<ChatPrefabManager>().Setmessage(message.sender, timeToDisplay, message.chatMessage);
    }

    void DisplayMessage(string message)
    {
        {
            GameObject chatMessage = Instantiate(chatMessagePrefab, chatDisplayContent.transform);

            string timeToDisplay = "[" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            chatMessage.GetComponent<ChatPrefabManager>().Setmessage("server", timeToDisplay, message);
        }
    }
}
