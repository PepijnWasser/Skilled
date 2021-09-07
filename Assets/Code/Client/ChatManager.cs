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
            lobbyState.SendChatRequest(inputText);
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
                        lobbyState.SendUpdateNameRequest(partsOfString[1]);
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

    void DisplayMessage(string message)
    {
        {
            GameObject chatMessage = Instantiate(chatMessagePrefab, chatDisplayContent.transform);

            string timeToDisplay = "[" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            chatMessage.GetComponent<ChatPrefabManager>().Setmessage("server", timeToDisplay, message);
        }
    }

    public void DisplayMessage(string sender, string message, int hourSend, int minuteSend, int secondSend)
    {
        GameObject chatMessage = Instantiate(chatMessagePrefab, chatDisplayContent.transform);

        string timeToDisplay = "[" + secondSend + ":" + minuteSend + ":" + secondSend;
        chatMessage.GetComponent<ChatPrefabManager>().Setmessage(sender, timeToDisplay, message);
    }
}
