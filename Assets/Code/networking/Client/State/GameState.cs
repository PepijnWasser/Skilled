using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;

public class GameState : State
{
    public MapGenerator mapGenerator;

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

    //sending
    void SendGameLoadedMessage()
    {
        GameLoadedMessage message = new GameLoadedMessage();
        clientnetwork.SendObject(message);
    }
}
