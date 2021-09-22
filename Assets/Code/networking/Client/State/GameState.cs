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
    }

    protected override void Recv(IAsyncResult res)
    {
        IPEndPoint RemoteEndPoint = new IPEndPoint(IPAddress.Any, 44455);
        byte[] received = NetworkUtils.Read(receiver.EndReceive(res, ref RemoteEndPoint));
        Debug.Log("data received");

        UDPPacket inPacket = new UDPPacket(received);
        var tempOBJ = inPacket.ReadObject();

        if (tempOBJ is UpdatePlayerPositionMessage)
        {
            UpdatePlayerPositionMessage message = tempOBJ as UpdatePlayerPositionMessage;
            Debug.Log(message.playerPosition);
        }

        receiver.BeginReceive(new AsyncCallback(Recv), null);
    }

    protected override void Update()
    {
        try
        {
            if (tcpClientnetwork != null && tcpClientnetwork.tcpClient.Connected && tcpClientnetwork.tcpClient.Available > 0)
            {
                byte[] inBytes = NetworkUtils.Read(tcpClientnetwork.tcpClient.GetStream());
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
                else if(tempOBJ is UpdatePlayerPositionTCP)
                {
                    Debug.Log("received player position");
                    UpdatePlayerPositionTCP message = tempOBJ as UpdatePlayerPositionTCP;
                    HandleupdatePlayerPosition(message);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (tcpClientnetwork.tcpClient.Connected)
            {
                tcpClientnetwork.tcpClient.Close();
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

    void HandleupdatePlayerPosition(UpdatePlayerPositionTCP message)
    {
        Debug.Log("updating position");
        gameManager.MovePlayer(message.playerID, message.playerPosition, message.playerRotation);
    }

    //sending
    void SendGameLoadedMessage()
    {
        GameLoadedMessage message = new GameLoadedMessage();
        tcpClientnetwork.SendObjectThroughTCP(message);
    }

    public void SendPlayerPosition(Vector3 position, Vector3 rotation)
    {
        UpdatePlayerPositionMessage message = new UpdatePlayerPositionMessage();
        message.playerPosition = position;
        message.playerRotation = rotation;
        udpClientnetwork.SendObjectThroughUDP(message);
    }
}
