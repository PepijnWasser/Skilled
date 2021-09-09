using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;

public class LobbyState : State
{
    private LobbyView lobbyView;

    public Text playerCountUIElement;
    public Text serverNameUIElement;

    public Button backButtonUIElement;

    protected override void Awake()
    {
        base.Awake();
        lobbyView = GetComponent<LobbyView>();
        backButtonUIElement.onClick.AddListener(SendLeaveServerMessage);
    }

    private void Start()
    {
        SendUpdateNameRequest(clientnetwork.playerName);
    }

    private void OnDestroy()
    {
        backButtonUIElement.onClick.RemoveListener(SendLeaveServerMessage);
    }

    protected override void Update()
    {
        try
        {
            if (clientnetwork != null && clientnetwork.client.Connected && clientnetwork.client.Available > 0)
            {
                byte[] inBytes = StreamUtil.Read(clientnetwork.client.GetStream());
                Packet inPacket = new Packet(inBytes);

                var tempOBJ = inPacket.ReadObject();

                if (tempOBJ is UpdatePlayerCountMessage)
                {
                    UpdatePlayerCountMessage message = tempOBJ as UpdatePlayerCountMessage;
                    HandlePlayerCountMessage(message);
                }
                else if (tempOBJ is HeartBeat)
                {
                    HandleHeartbeat();
                }
                else if (tempOBJ is UpdateServerNameMessage)
                {
                    UpdateServerNameMessage message = tempOBJ as UpdateServerNameMessage;
                    HandleServerNameMessage(message);
                }
                else if (tempOBJ is UpdateColorRespons)
                {
                    UpdateColorRespons message = tempOBJ as UpdateColorRespons;
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
                else if (tempOBJ is ChatRespons)
                {
                    ChatRespons message = tempOBJ as ChatRespons;
                    HandleChatMessage(message);
                }
                else if (tempOBJ is HelpRespons)
                {
                    HelpRespons message = tempOBJ as HelpRespons;
                    HandleHelpRespons(message);
                }
                else if(tempOBJ is ServerOwnerMessage)
                {
                    ServerOwnerMessage message = tempOBJ as ServerOwnerMessage;
                    HandleServerOwnerMessage(message);
                }
                else if(tempOBJ is JoinRoomMessage)
                {
                    JoinRoomMessage message = tempOBJ as JoinRoomMessage;
                    HandleJoinRoomMessage(message);
                }
            }
            HandleHeartbeatStatus();
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

    //
    //Handling messages from the server
    //
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

    void HandleUpdateColorMessage(UpdateColorRespons message)
    {
        lobbyView.UpdatePlayerColor(message.playerID, message.color.ToString());
    }

    void HandleUpdatePlayerNameRespons(UpdatePlayerNameRespons message)
    {
        lobbyView.UpdateName(message.playerID, message.playerName);
    }

    void HandleChatMessage(ChatRespons message)
    {
        GameObject.FindObjectOfType<ChatManager>().DisplayMessage(message.sender, message.chatMessage, message.hourSend, message.minuteSend, message.secondSend);
    }

    void HandleHelpRespons(HelpRespons message)
    {
        GameObject.FindObjectOfType<ChatManager>().DisplayMessage(message.sender, message.message, message.hourSend, message.minuteSend, message.secondSend);
    }

    void HandleServerOwnerMessage(ServerOwnerMessage message)
    {
        lobbyView.SetServerOwner(message.isOwner);
    }

    void HandleJoinRoomMessage(JoinRoomMessage message)
    {
        if(message.roomToJoin == JoinRoomMessage.rooms.game)
        {
            GameObject.FindObjectOfType<MySceneManager>().LoadScene("GameScene");
        }
        else
        {
            Debug.Log("request to join room failed");
        }
    }

    //
    //Sending Messages to the server
    //
    public void SendUpdateNameRequest(string newName)
    {
        UpdatePlayerNameRequest request = new UpdatePlayerNameRequest(newName);
        clientnetwork.SendObject(request);
    }

    public void SendUpdateColorRequest(int sideToChangeTo)
    {
        UpdateColorRequest request = new UpdateColorRequest(sideToChangeTo);
        clientnetwork.SendObject(request);
    }

    public void SendChatRequest(string _chatMessage)
    {
        ChatRequest request = new ChatRequest(_chatMessage);
        clientnetwork.SendObject(request);
    }

    public void SendHelpRequest()
    {
        HelpRequest request = new HelpRequest();
        clientnetwork.SendObject(request);
    }

    public void SendStartRoomRequest()
    {
        StartRoomRequest request = new StartRoomRequest();
        clientnetwork.SendObject(request);
    }

    void SendLeaveServerMessage()
    {
        LeaveServermessage message = new LeaveServermessage();
        clientnetwork.SendObject(message);
    }

    void HandleHeartbeatStatus()
    {
        if(CheckHeartbeat() == false)
        {
            SendLeaveServerMessage();
            GetComponent<CreateCJScreen>().CreateCJScreenItem();
            Destroy(this.gameObject);
        }
    }
}
