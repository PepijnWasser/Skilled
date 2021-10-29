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

    //sends the playerInfo from the inputWindow to the server
    private void Start()
    {
        SendUpdateNameRequest(PlayerInfo.playerName);
    }

    private void OnDestroy()
    {
        backButtonUIElement.onClick.RemoveListener(SendLeaveServerMessage);
    }

    //handles tcp data from server
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
                    RefreshHeartbeat();
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

    //sets the correct player count in the view
    void HandlePlayerCountMessage(UpdatePlayerCountMessage message)
    {
        playerCountUIElement.text = message.playerCount.ToString();
    }

    //sets the variables of the view to the correct values and save the data in the serverConnectionData
    void HandleServerInfoMessage(UpdateServerInfo message)
    {
        serverNameUIElement.text = message.owner + "'s server";
        lobbyView.SetServerOwner(message.isOwner);
        lobbyView.SetServerIP(message.ip.ToString());
        lobbyView.SetServerPort(message.tcpPort);

        ServerConnectionData.owner = message.owner;
        ServerConnectionData.isOwner = message.isOwner;
        ServerConnectionData.udpPort = message.udpPort;
        ServerConnectionData.tcpPort = message.tcpPort;
        ServerConnectionData.ip = message.ip;
       
    }

    //makes a new player bar
    //if we are the owner, set the playerInfo
    void HandleMakeNewPlayerMessage(MakeNewPlayerBarMessage message)
    {
        lobbyView.AddPlayer(message.playerID, message.playerColor, message.playerName, message.isPlayer);
        if (message.isPlayer)
        {
            PlayerInfo.playerID = message.playerID;
        }
    }

    //removes a player from the view
    void HandleRemovePlayerMessage(RemovePlayerBarMessage message)
    {
        lobbyView.RemovePlayer(message.playerID);
    }

    //sets tje color of a player in the view
    void HandleUpdateColorMessage(UpdateColorRespons message)
    {
        lobbyView.UpdatePlayerColor(message.playerID, message.color.ToString());
    }

    //updates the name of a player in the view
    void HandleUpdatePlayerNameRespons(UpdatePlayerNameRespons message)
    {
        lobbyView.UpdateName(message.playerID, message.playerName);

        if (message.playerID == PlayerInfo.playerID)
        {
            PlayerInfo.playerName = message.playerName;
        }
    }

    //displays the chatmessage in the view
    void HandleChatMessage(ChatRespons message)
    {
        GameObject.FindObjectOfType<ChatManager>().DisplayMessage(message.sender, message.chatMessage, message.hourSend, message.minuteSend, message.secondSend);
    }

    //displays help commands in the chat
    void HandleHelpRespons(HelpRespons message)
    {
        GameObject.FindObjectOfType<ChatManager>().DisplayMessage(message.sender, message.message, message.hourSend, message.minuteSend, message.secondSend);
    }

    //loads the game scene
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

    //if the server has no more heartbeat, we leave the server
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
