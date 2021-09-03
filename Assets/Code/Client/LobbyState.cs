using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;

public class LobbyState : MonoBehaviour
{
    private LocalHostClient clientnetwork;
    private LobbyView lobbyView;

    public Text playerCountUIElement;
    public Text serverNameUIElement;

    public Button backButtonUIElement;

    private void Awake()
    {
        clientnetwork = GameObject.FindObjectOfType<LocalHostClient>().GetComponent<LocalHostClient>();
        lobbyView = GetComponent<LobbyView>();
        backButtonUIElement.onClick.AddListener(SendLeaveServerMessage);
    }

    private void OnDestroy()
    {
        backButtonUIElement.onClick.RemoveListener(SendLeaveServerMessage);
    }

    private void Update()
    {
        try
        {
            if (clientnetwork != null && clientnetwork.client.Connected && clientnetwork.client.Available > 0)
            {
                byte[] inBytes = StreamUtil.Read(clientnetwork.client.GetStream());
                Packet inPacket = new Packet(inBytes);

                var tempOBJ = inPacket.ReadObject();

                Debug.Log("Received Message of type: " + tempOBJ.GetType());


                if (tempOBJ is UpdatePlayerCountMessage)
                {
                    UpdatePlayerCountMessage message = tempOBJ as UpdatePlayerCountMessage;
                    HandlePlayerCountMessage(message);
                }
                else if (tempOBJ is UpdateServerNameMessage)
                {
                    UpdateServerNameMessage message = tempOBJ as UpdateServerNameMessage;
                    HandleServerNameMessage(message);
                }
                else if (tempOBJ is UpdateColorMessage)
                {
                    UpdateColorMessage message = tempOBJ as UpdateColorMessage;
                    HandleUpdateColorMessage(message);
                }
                else if (tempOBJ is MakeNewPlayerBarMessage)
                {
                    MakeNewPlayerBarMessage message = tempOBJ as MakeNewPlayerBarMessage;
                    HandleMakeNewPlayerMessage(message);
                }
                else if (tempOBJ is RemovePlayerBarMessage)
                {
                    RemovePlayerBarMessage message = tempOBJ as RemovePlayerBarMessage;
                    HandleRemovePlayerMessage(message);
                }
                else if (tempOBJ is UpdatePlayerNameRespons)
                {
                    UpdatePlayerNameRespons message = tempOBJ as UpdatePlayerNameRespons;
                    HandleUpdatePlayerNameRespons(message);
                }
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (clientnetwork.client.Connected)
            {
                clientnetwork.client.Close();
            }
        }
    }

    void HandlePlayerCountMessage(UpdatePlayerCountMessage message)
    {
        playerCountUIElement.text = message.playerCount.ToString();
    }

    void HandleServerNameMessage(UpdateServerNameMessage message)
    {
        serverNameUIElement.text = message.serverName + "'s server";
    }

    void HandleMakeNewPlayerMessage(MakeNewPlayerBarMessage message)
    {
        lobbyView.AddPlayer(message.playerID, message.playerColor, message.playerName, message.isPlayer);
    }

    void HandleRemovePlayerMessage(RemovePlayerBarMessage message)
    {
        lobbyView.RemovePlayer(message.playerID);
    }

    void HandleUpdateColorMessage(UpdateColorMessage message)
    {
        lobbyView.UpdatePlayerColor(message.playerID, message.color);
    }

    void HandleUpdatePlayerNameRespons(UpdatePlayerNameRespons message)
    {
        lobbyView.UpdateName(message.playerID, message.playerName);
    }

    public void UpdatePlayerColorRequest(int sideToChangeTo)
    {
        RequestColorChangeMessage message = new RequestColorChangeMessage(sideToChangeTo);
        clientnetwork.SendObject(message);
    }

    void SendLeaveServerMessage()
    {
        LeaveServermessage message = new LeaveServermessage();
        clientnetwork.SendObject(message);
    }
}
