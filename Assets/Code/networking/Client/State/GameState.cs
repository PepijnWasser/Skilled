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
                else if(tempOBJ is UpdatePlayerPositionTCP)
                {
                    UpdatePlayerPositionTCP message = tempOBJ as UpdatePlayerPositionTCP;
                    HandleupdatePlayerPosition(message);
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

    void HandleupdatePlayerPosition(UpdatePlayerPositionTCP message)
    {
        gameManager.MovePlayer(message.playerID, message.playerPosition, message.playerRotation);
    }

    //sending
    void SendGameLoadedMessage()
    {
        GameLoadedMessage message = new GameLoadedMessage();
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
