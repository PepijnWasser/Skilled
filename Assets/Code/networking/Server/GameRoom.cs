using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;

public class GameRoom : Room
{
    int gameLoadedMessages = 0;
    int worldSeed = 20;
    int amountOfSectors = 3;


    public override void handleTCPNetworkMessageFromUser(ISerializable tempOBJ, MyClient client)
    {
        if (tempOBJ is HeartBeat)
        {
            RefreshHeartbeat(client);
        }
        else if (tempOBJ is GameLoadedMessage)
        {
            HandleGameLoadedMessage(client);
        }
        else if(tempOBJ is UpdatePlayerPositionMessage)
        {
            UpdatePlayerPositionMessage message = tempOBJ as UpdatePlayerPositionMessage;
            HandleUpdatePlayerPositionMessage(message, client);
        }
    }

    protected override void CheckHeartbeat()
    {
        List<MyClient> clientsInRoomToRemove = new List<MyClient>();

        for (int i = 0; i < clientsInRoom.Count; i++)
        {
            clientsInRoom[i].heartbeat -= Time.deltaTime;
            if (clientsInRoom[i].heartbeat <= 0)
            {
                clientsInRoomToRemove.Add(clientsInRoom[i]);
            }
        }

        foreach (MyClient client in clientsInRoomToRemove)
        {
            Debug.Log("timeout " + client.playerName);
            SendPlayerDisconnectMessages(client.playerName);
            removeAndCloseMember(client);
        }
    }

    void SendPlayerDisconnectMessages(string playerWhoDisconnected)
    {
        DateTime time = DateTime.Now;
        string messageToSend = playerWhoDisconnected + " timed out";

        Packet outPacket = new Packet();
        ChatRespons chatMessage = new ChatRespons(messageToSend, "Server", time.Hour, time.Minute, time.Second);
        outPacket.Write(chatMessage);
        SendTCPMessageToAllUsers(outPacket);
    }

    private void RefreshHeartbeat(MyClient pClient)
    {
        pClient.heartbeat = server.timeOutTime;
    }

    public override void UpdateRoom()
    {
        base.UpdateRoom();
    }

    void HandleGameLoadedMessage(MyClient client)
    {
        gameLoadedMessages += 1;

        Packet outPacket = new Packet();
        MakeGameMapMessage makeGameMapMessage = new MakeGameMapMessage(worldSeed, amountOfSectors);
        outPacket.Write(makeGameMapMessage);
        SendTCPMessageToTargetUser(outPacket, client);

        if (gameLoadedMessages == clientsInRoom.Count)
        {

        }
        
        Packet outpacket = new Packet();
        MakenewPlayerCharacterMessage makePlayerCharacterMessage = new MakenewPlayerCharacterMessage(true, Vector3.zero, client.playerID, client.playerName);
        outpacket.Write(makePlayerCharacterMessage);
        SendTCPMessageToTargetUser(outpacket, client);
            
        Packet outpacket2 = new Packet();
        MakenewPlayerCharacterMessage makePlayerCharacterMessage2 = new MakenewPlayerCharacterMessage(false, Vector3.zero, client.playerID, client.playerName);
        outpacket2.Write(makePlayerCharacterMessage2);
        SendTCPMessageToAllUsersExcept(outpacket2, client);
    }

    void HandleUpdatePlayerPositionMessage(UpdatePlayerPositionMessage message, MyClient client)
    {
        Packet outPacket = new Packet();
        UpdatePlayerPositionMessage outMessage = new UpdatePlayerPositionMessage(message.playerPosition, client.playerID);
        outPacket.Write(outMessage);
        SendTCPMessageToAllUsersExcept(outPacket, client);
    }
}
