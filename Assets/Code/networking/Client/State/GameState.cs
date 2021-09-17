using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;

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
            if (clientnetwork != null && clientnetwork.tcpClient.Connected && clientnetwork.tcpClient.Available > 0)
            {
                byte[] inBytes = NetworkUtils.Read(clientnetwork.tcpClient.GetStream());
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
                else if(tempOBJ is UpdatePlayerPositionMessage)
                {
                    UpdatePlayerPositionMessage message = tempOBJ as UpdatePlayerPositionMessage;
                    HandleupdatePlayerPosition(message);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            if (clientnetwork.tcpClient.Connected)
            {
                clientnetwork.tcpClient.Close();
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

    void HandleupdatePlayerPosition(UpdatePlayerPositionMessage message)
    {
        gameManager.MovePlayer(message.playerID, message.playerPosition, message.playerRotation);
    }

    //sending
    void SendGameLoadedMessage()
    {
        GameLoadedMessage message = new GameLoadedMessage();
        clientnetwork.SendObjectThroughTCP(message);
    }

    public void SendPlayerPosition(Vector3 position, Vector3 rotation)
    {
        UpdatePlayerPositionMessage message = new UpdatePlayerPositionMessage();
        message.playerPosition = position;
        message.playerRotation = rotation;
        clientnetwork.SendObjectThroughTCP(message);
    }
}
