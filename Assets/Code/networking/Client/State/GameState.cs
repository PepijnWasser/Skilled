using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;
using System.Net;
using System.Net.Sockets;

public class GameState : State
{
    public MapGenerator mapGenerator;
    public GameManager gameManager;

    private void Start()
    {
        SendGameLoadedMessage();

        int i = 0;
        bool finishedInitialization = false;

        while (finishedInitialization == false && i < 20)
        {
            try
            {
                client = new UdpClient(40004 + i);
                Debug.Log("listening on " + Extensions.GetLocalIPAddress() + " port " + 40004 + i);

                PlayerInfo playerInfo = GameObject.FindObjectOfType<PlayerInfo>();
                playerInfo.udpReceivePort = 40004 + i;

                finishedInitialization = true;
            }
            catch (Exception e)
            {
                e.ToString();
                i++;
            }
        }
        client.BeginReceive(new AsyncCallback(recv), null);
        SendPlayerInfo();
    }

    protected override void recv(IAsyncResult res)
    {
        IPEndPoint RemoteIP = new IPEndPoint(IPAddress.Any, 60240);
        byte[] received = client.EndReceive(res, ref RemoteIP);

        UDPPacket packet = new UDPPacket(received);
        var TempOBJ = packet.ReadObject();
        if(TempOBJ is UpdatePlayerPositionUDP)
        {
            UpdatePlayerPositionUDP message = TempOBJ as UpdatePlayerPositionUDP;
            HandleUpdatePlayerPosition(message);
        }

        client.BeginReceive(new AsyncCallback(recv), null);
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

                if (tempOBJ is MakeGameMapMessage)
                {
                    MakeGameMapMessage message = tempOBJ as MakeGameMapMessage;
                    HandleMakeGameMapMessage(message);
                }
                else if (tempOBJ is MakenewPlayerCharacterMessage)
                {
                    MakenewPlayerCharacterMessage message = tempOBJ as MakenewPlayerCharacterMessage;
                    HandleMakePlayerCharacterMessage(message);
                }
            }
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
    //handle receiving
    void HandleMakeGameMapMessage(MakeGameMapMessage message)
    {
        Extensions.SetSeed(message.worldSeed);
        mapGenerator.amountOfSectors = message.amountOfSectors;
    }

    void HandleMakePlayerCharacterMessage(MakenewPlayerCharacterMessage message)
    {
        gameManager.MakePlayerCharacter(message.isPlayer, message.characterPosition, message.playerName, message.playerID);
    }

    void HandleUpdatePlayerPosition(UpdatePlayerPositionUDP message)
    {
        try
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                // Will run on main thread, hence issue is solved
                gameManager.MovePlayer(message.playerID, message.playerPosition, message.playerRotation);
            });           
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    //sending
    void SendGameLoadedMessage()
    {
        GameLoadedMessage message = new GameLoadedMessage();
        tcpClientNetwork.SendObjectThroughTCP(message);
    }

    void SendPlayerInfo()
    {
        UpdateClientInfoMessage message = new UpdateClientInfoMessage(Extensions.GetLocalIPAddress(), playerInfo.udpSendPort, playerInfo.udpReceivePort);
        tcpClientNetwork.SendObjectThroughTCP(message);
    }

    public void SendPlayerPosition(Vector3 position, Vector3 rotation)
    {
        
        UpdatePlayerPositionUDP message = new UpdatePlayerPositionUDP();
        message.playerPosition = position;
        message.playerRotation = rotation;
        udpClientNetwork.SendObjectThroughUDP(message);       
    }
}
