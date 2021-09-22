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


    public override void handleUDPNetworkMessageFromUser(USerializable tempOBJ, MyClient client)
    {
        if (tempOBJ is HeartBeat)
        {
            RefreshHeartbeat(client);
        }
        else if (tempOBJ is UpdatePlayerPositionMessage)
        { 
            UpdatePlayerPositionMessage message = tempOBJ as UpdatePlayerPositionMessage;
            HandleUpdatePlayerPositionMessage(message, client);
        }
    }

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

        TCPPacket outPacket = new TCPPacket();
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

        TCPPacket outPacket = new TCPPacket();
        MakeGameMapMessage makeGameMapMessage = new MakeGameMapMessage(worldSeed, amountOfSectors);
        outPacket.Write(makeGameMapMessage);
        SendTCPMessageToTargetUser(outPacket, client);

        if (gameLoadedMessages == clientsInRoom.Count)
        {

        }
        
        TCPPacket outpacket = new TCPPacket();
        MakenewPlayerCharacterMessage makePlayerCharacterMessage = new MakenewPlayerCharacterMessage(true, Vector3.zero, client.playerID, client.playerName);
        outpacket.Write(makePlayerCharacterMessage);
        SendTCPMessageToTargetUser(outpacket, client);
            
        TCPPacket outpacket2 = new TCPPacket();
        MakenewPlayerCharacterMessage makePlayerCharacterMessage2 = new MakenewPlayerCharacterMessage(false, Vector3.zero, client.playerID, client.playerName);
        outpacket2.Write(makePlayerCharacterMessage2);
        SendTCPMessageToAllUsersExcept(outpacket2, client);
    }

    void HandleUpdatePlayerPositionMessage(UpdatePlayerPositionMessage message, MyClient client)
    {
        Debug.Log("message is update position");
        /*
        UDPPacket outPacket = new UDPPacket();
        UpdatePlayerPositionMessage outMessage = new UpdatePlayerPositionMessage(message.playerPosition, message.playerRotation, client.playerID);
        outPacket.Write(outMessage);
        //SendUDPMessageToAllUsersExcept(outPacket, client);
        SendUDPMessageToAllUsers(outPacket);
        */

        TCPPacket outpacket = new TCPPacket();
        UpdatePlayerPositionTCP messagre = new UpdatePlayerPositionTCP(message.playerPosition, message.playerRotation, client.playerID);
        outpacket.Write(messagre);
        SendTCPMessageToAllUsers(outpacket);
    }
}
