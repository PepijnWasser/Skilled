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
    int mapMadeMessages = 0;
    int worldSeed = Random.Range(1, 30);
    int amountOfSectors = 1;

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
        else if(tempOBJ is MapMadeMessage)
        {
            MapMadeMessage message = tempOBJ as MapMadeMessage;
            HandleMapMadeMessage(message, myClient);
        }
        else if(tempOBJ is UpdateStationHealthRequest)
        {
            UpdateStationHealthRequest message = tempOBJ as UpdateStationHealthRequest;
            HandleUpdateStationHealthRequest(message, myClient);
        }
        else if(tempOBJ is AddKeypadTaskMessage)
        {
            AddKeypadTaskMessage message = tempOBJ as AddKeypadTaskMessage;
            HandleAddKeypadTask(message, myClient);
        }
        else if(tempOBJ is AddTwoWayLeverTask)
        {
            AddTwoWayLeverTask message = tempOBJ as AddTwoWayLeverTask;
            HandleAddTwoWayLeverTask(message, myClient);
        }
        else if (tempOBJ is AddThreeWayLeverTask)
        {
            AddThreeWayLeverTask message = tempOBJ as AddThreeWayLeverTask;
            HandleAddThreeWayLeverTask(message, myClient);
        }
        else if(tempOBJ is UpdateTwoWayLeverPositionMessage)
        {
            UpdateTwoWayLeverPositionMessage message = tempOBJ as UpdateTwoWayLeverPositionMessage;
            HandleUpdateTwoWayLeverPosition(message, myClient);
        }
        else if (tempOBJ is UpdateThreeWayLeverPositionMessage)
        {
            UpdateThreeWayLeverPositionMessage message = tempOBJ as UpdateThreeWayLeverPositionMessage;
            HandleUpdateThreeWayLeverPosition(message, myClient);
        }
        else if(tempOBJ is UpdateKeypadStatusMessage)
        {
            UpdateKeypadStatusMessage message = tempOBJ as UpdateKeypadStatusMessage;
            HandleUpdatekeypadStatus(message, myClient);
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
        TCPPacket outPacket = new TCPPacket();
        MakeGameMapMessage makeGameMapMessage = new MakeGameMapMessage(worldSeed, amountOfSectors);
        outPacket.Write(makeGameMapMessage);
        SendTCPMessageToTargetUser(outPacket, client);
    }

    void HandleUpdatePlayerPositionMessageUDP(UpdatePlayerPositionUDP _message, MyClient client)
    {  
        UDPPacket outpacket = new UDPPacket();
        UpdatePlayerPositionUDP messagre = new UpdatePlayerPositionUDP(_message.playerPosition, _message.playerRotation, _message.playerNoseRotation, client.playerID);
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

    void HandleMapMadeMessage(MapMadeMessage message, MyClient client)
    {
        mapMadeMessages += 1;

        if (mapMadeMessages == clientsInRoom.Count)
        {
            MakeTasks();
            MakePlayerCharacters();
        }
    }

    void MakePlayerCharacters()
    {
        foreach (MyClient clientToSendTo in clientsInRoom)
        {
            foreach (MyClient clientToSend in clientsInRoom)
            {
                if (clientToSend != clientToSendTo)
                {
                    TCPPacket outpacket2 = new TCPPacket();
                    MakenewPlayerCharacterMessage makePlayerCharacterMessage2 = new MakenewPlayerCharacterMessage(false, clientToSend.playerPosition, clientToSend.playerID, clientToSend.playerName);
                    outpacket2.Write(makePlayerCharacterMessage2);
                    SendTCPMessageToTargetUser(outpacket2, clientToSendTo);
                }
                else
                {
                    TCPPacket outpacket = new TCPPacket();
                    MakenewPlayerCharacterMessage makePlayerCharacterMessage = new MakenewPlayerCharacterMessage(true, clientToSend.playerPosition, clientToSend.playerID, clientToSend.playerName);
                    outpacket.Write(makePlayerCharacterMessage);
                    SendTCPMessageToTargetUser(outpacket, clientToSendTo);

                }
            }
        }
    }

    void MakeTasks()
    {
        TCPPacket outpacket = new TCPPacket();
        MakeTaskManager makeTaskManagerMessage = new MakeTaskManager(false);
        outpacket.Write(makeTaskManagerMessage);
        SendTCPMessageToAllUsersExcept(outpacket, server.serverInfo.serverOwner);

        TCPPacket outpacket2 = new TCPPacket();
        MakeTaskManager makeTaskManagerMessage2 = new MakeTaskManager(true);
        outpacket2.Write(makeTaskManagerMessage2);
        SendTCPMessageToTargetUser(outpacket2, server.serverInfo.serverOwner);
    }

    void HandleUpdateStationHealthRequest(UpdateStationHealthRequest message, MyClient client)
    {
        TCPPacket outPacket = new TCPPacket();
        UpdateStationHealthResponse updateStationHealthResponse = new UpdateStationHealthResponse(message.stationHealth);
        outPacket.Write(updateStationHealthResponse);
        SendTCPMessageToAllUsersExcept(outPacket, client);
    }

    void HandleAddKeypadTask(AddKeypadTaskMessage message, MyClient client)
    {
        TCPPacket outpacket = new TCPPacket();
        outpacket.Write(message);
        SendTCPMessageToAllUsersExcept(outpacket, client);
    }

    void HandleAddTwoWayLeverTask(AddTwoWayLeverTask message, MyClient client)
    {
        TCPPacket outpacket = new TCPPacket();
        outpacket.Write(message);
        SendTCPMessageToAllUsersExcept(outpacket, client);
    }

    void HandleAddThreeWayLeverTask(AddThreeWayLeverTask message, MyClient client)
    {
        TCPPacket outpacket = new TCPPacket();
        outpacket.Write(message);
        SendTCPMessageToAllUsersExcept(outpacket, client);
    }

    void HandleUpdateTwoWayLeverPosition(UpdateTwoWayLeverPositionMessage message, MyClient client)
    {
        TCPPacket outPacket = new TCPPacket();
        outPacket.Write(message);
        SendTCPMessageToAllUsers(outPacket);
    }

    void HandleUpdateThreeWayLeverPosition(UpdateThreeWayLeverPositionMessage message, MyClient client)
    {
        TCPPacket outPacket = new TCPPacket();
        outPacket.Write(message);
        SendTCPMessageToAllUsers(outPacket);
    }

    void HandleUpdatekeypadStatus(UpdateKeypadStatusMessage message, MyClient client)
    {
        TCPPacket outPacket = new TCPPacket();
        outPacket.Write(message);
        SendTCPMessageToAllUsers(outPacket);
    }
}
