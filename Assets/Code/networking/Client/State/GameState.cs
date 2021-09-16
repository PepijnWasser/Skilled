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
            if (clientnetwork != null && clientnetwork.client.Connected && clientnetwork.client.Available > 0)
            {
                byte[] inBytes = StreamUtil.Read(clientnetwork.client.GetStream());
                Packet inPacket = new Packet(inBytes);

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
    //handle receiving
    void HandleMakeGameMapMessage(MakeGameMapMessage message)
    {
        Extensions.SetSeed(message.worldSeed);
        mapGenerator.amountOfSectors = message.amountOfSectors;
    }

    void HandleMakePlayerCharacterMessage(MakenewPlayerCharacterMessage message)
    {
        gameManager.MakePlayerCharacter(message.isPlayer, message.characterPosition, message.playerName);
    }

    void HandleupdatePlayerPosition(UpdatePlayerPositionMessage message)
    {

    }

    //sending
    void SendGameLoadedMessage()
    {
        GameLoadedMessage message = new GameLoadedMessage();
        clientnetwork.SendObject(message);
    }

    public void SendPlayerPosition(Vector3 position)
    {
        UpdatePlayerPositionMessage message = new UpdatePlayerPositionMessage();
        message.playerPosition = position;
        clientnetwork.SendObject(message);
    }
}
