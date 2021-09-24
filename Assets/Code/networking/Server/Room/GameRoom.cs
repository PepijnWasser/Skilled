using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;

using Random = UnityEngine.Random;
public class GameRoom : Room
{
    int gameLoadedMessages = 0;
    int worldSeed = 20;
    int amountOfSectors = 3;

    public override void handleUDPNetworkMessageFromUser(USerializable pMessage, MyClient pSender)
    {
        if(pMessage is UDPMessage)
        {
            UDPMessage message = pMessage as UDPMessage;
            Debug.Log(message.message);
        }
        else if(pMessage is UpdatePlayerPositionUDP)
        {
            UpdatePlayerPositionUDP message = pMessage as UpdatePlayerPositionUDP;
            HandleUpdatePlayerPositionMessageUDP(message, pSender);
        }
    }

    public override void handleTCPNetworkMessageFromUser(ISerializable tempOBJ, MyClient myClient)
    {
        if (tempOBJ is HeartBeat)
        {
            RefreshHeartbeat(myClient);
        }
        else if (tempOBJ is GameLoadedMessage)
        {
            HandleGameLoadedMessage(myClient);
        }
        else if(tempOBJ is UpdateClientInfoMessage)
        {
            UpdateClientInfoMessage message = tempOBJ as UpdateClientInfoMessage;
            HandleUpdateClientInfo(message, myClient);
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
        MakenewPlayerCharacterMessage makePlayerCharacterMessage = new MakenewPlayerCharacterMessage(true, client.playerPosition, client.playerID, client.playerName);
        outpacket.Write(makePlayerCharacterMessage);
        SendTCPMessageToTargetUser(outpacket, client);
            

        foreach(MyClient myClient in clientsInRoom)
        {
            if(myClient != client)
            {
                TCPPacket outpacket2 = new TCPPacket();
                MakenewPlayerCharacterMessage makePlayerCharacterMessage2 = new MakenewPlayerCharacterMessage(false, client.playerPosition, client.playerID, client.playerName);
                outpacket2.Write(makePlayerCharacterMessage2);
                SendTCPMessageToAllUsersExcept(outpacket2, client);
            }
        }

    }

    void HandleUpdatePlayerPositionMessageUDP(UpdatePlayerPositionUDP _message, MyClient client)
    {  
        UDPPacket outpacket = new UDPPacket();
        UpdatePlayerPositionUDP messagre = new UpdatePlayerPositionUDP(_message.playerPosition, _message.playerRotation, client.playerID);
        outpacket.Write(messagre);

        foreach(MyClient c in clientsInRoom)
        {
            if(c != client)
            {
                Debug.Log("sending to " + c.endPoint);
            }
        }
        SendUDPMessageToAllUsersExcept(outpacket, client);
    }

    void HandleUpdateClientInfo(UpdateClientInfoMessage message, MyClient client)
    {
        client.endPoint = new IPEndPoint(message.ip, message.receivePort);
        client.sendPort = message.sendPort;
    }
}
