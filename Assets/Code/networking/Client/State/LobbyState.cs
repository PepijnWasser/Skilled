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
        SendUpdateNameRequest(playerInfo.playerName);
    }

    private void OnDestroy()
    {
        backButtonUIElement.onClick.RemoveListener(SendLeaveServerMessage);
    }

    protected override void Update()
    {
        try
        {
            if (tcpClientNetwork != null && tcpClientNetwork.tcpClient.Connected && tcpClientNetwork.tcpClient.Available > 0)
            {
                byte[] inBytes = NetworkUtils.Read(tcpClientNetwork.tcpClient.GetStream());
                TCPPacket inPacket = new TCPPacket(inBytes);

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
                else if(tempOBJ is UpdateServerInfo)
                {
                    UpdateServerInfo message = tempOBJ as UpdateServerInfo;
                    HandleServerInfoMessage(message);
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
            if (tcpClientNetwork.tcpClient.Connected)
            {
                tcpClientNetwork.tcpClient.Close();
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


    void HandleServerInfoMessage(UpdateServerInfo message)
    {
        serverNameUIElement.text = message.owner + "'s server";
        lobbyView.SetServerOwner(message.isOwner);
        lobbyView.SetServerIP(message.ip.ToString());
        lobbyView.SetServerPort(message.tcpPort);

        ServerConnectionData serverData = GameObject.FindObjectOfType<ServerConnectionData>();
        serverData.owner = message.owner;
        serverData.isOwner = message.isOwner;
        serverData.udpPort = message.udpPort;
        serverData.tcpPort = message.tcpPort;
        serverData.ip = message.ip;
       
    }

    void HandleMakeNewPlayerMessage(MakeNewPlayerBarMessage message)
    {
        lobbyView.AddPlayer(message.playerID, message.playerColor, message.playerName, message.isPlayer);
        if (message.isPlayer)
        {
            PlayerInfo playerInfo = GameObject.FindObjectOfType<PlayerInfo>();
            playerInfo.playerID = message.playerID;
        }
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

        PlayerInfo playerInfo = GameObject.FindObjectOfType<PlayerInfo>();
        if (message.playerID == playerInfo.playerID)
        {
            playerInfo.playerName = message.playerName;
        }
    }

    void HandleChatMessage(ChatRespons message)
    {
        GameObject.FindObjectOfType<ChatManager>().DisplayMessage(message.sender, message.chatMessage, message.hourSend, message.minuteSend, message.secondSend);
    }

    void HandleHelpRespons(HelpRespons message)
    {
        GameObject.FindObjectOfType<ChatManager>().DisplayMessage(message.sender, message.message, message.hourSend, message.minuteSend, message.secondSend);
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
        tcpClientNetwork.SendObjectThroughTCP(request);
    }

    public void SendUpdateColorRequest(int sideToChangeTo)
    {
        UpdateColorRequest request = new UpdateColorRequest(sideToChangeTo);
        tcpClientNetwork.SendObjectThroughTCP(request);
    }

    public void SendChatRequest(string _chatMessage)
    {
        ChatRequest request = new ChatRequest(_chatMessage);
        tcpClientNetwork.SendObjectThroughTCP(request);
    }

    public void SendHelpRequest()
    {
        HelpRequest request = new HelpRequest();
        tcpClientNetwork.SendObjectThroughTCP(request);
    }

    public void SendStartRoomRequest()
    {
        StartRoomRequest request = new StartRoomRequest();
        tcpClientNetwork.SendObjectThroughTCP(request);
    }

    void SendLeaveServerMessage()
    {
        LeaveServermessage message = new LeaveServermessage();
        tcpClientNetwork.SendObjectThroughTCP(message);
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
