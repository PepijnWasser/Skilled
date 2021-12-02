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
    int roomsPerSector = 10;
    int tasksOfTypeToSpawn = 0;
    int maxErrors = 0;

    int tasksCompleted = 0;

    public void SetRoomsAmount(int amountOfPlayers)
    {
        amountOfSectors = (int)Mathf.Ceil((float)amountOfPlayers / (float)3);
        roomsPerSector = 20 + 5 * amountOfPlayers;
        tasksOfTypeToSpawn = (int)Mathf.Ceil((float)roomsPerSector * amountOfSectors * (float)0.1 / (float)3);
        maxErrors = (int)Mathf.Ceil((float)tasksOfTypeToSpawn / (float)2);
    }

    public override void RemoveMember(MyClient clientToRemove)
    {
        base.RemoveMember(clientToRemove);
        TCPPacket outPacket = new TCPPacket();
        RemovePlayerCharacterMessage message = new RemovePlayerCharacterMessage(clientToRemove.playerID);
        outPacket.Write(message);
        SendTCPMessageToAllUsers(outPacket);
    }

    //processes udp message of a given user
    public override void HandleUDPNetworkMessageFromUser(USerializable pMessage, MyClient pSender)
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
        else if(pMessage is UpdatePlayerCamPosition)
        {
            UpdatePlayerCamPosition message = pMessage as UpdatePlayerCamPosition;
            HandleUpdatePlayerCamPosition(message, pSender);
        }
        else if(pMessage is UpdateEnergyCamPosition)
        {
            UpdateEnergyCamPosition message = pMessage as UpdateEnergyCamPosition;
            HandleUpdateEnergyCamPosition(message, pSender);
        }
        else if (pMessage is UpdateTaskCamPosition)
        {
            UpdateTaskCamPosition message = pMessage as UpdateTaskCamPosition;
            HandleUpdateTaskCamPosition(message, pSender);
        }
        else if (pMessage is UpdateRigidbodyPositionRequest)
        {
            UpdateRigidbodyPositionRequest message = pMessage as UpdateRigidbodyPositionRequest;
            HandleUpdateRigidbodyPositionRequest(message, pSender);
        }
    }

    //processes tcp message of a given user 
    public override void HandleTCPNetworkMessageFromUser(ISerializable tempOBJ, MyClient myClient)
    {
        if (tempOBJ is HeartBeat)
        {
            RefreshHeartbeat(myClient);
        }
        else if (tempOBJ is SceneLoadedMessage)
        {
            SceneLoadedMessage message = tempOBJ as SceneLoadedMessage;
            HandleSceneLoadedMessage(message, myClient);
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
        else if(tempOBJ is TwoWayLeverCompletedMessage)
        {
            TwoWayLeverCompletedMessage message = tempOBJ as TwoWayLeverCompletedMessage;
            HandleTwoWayLeverTaskCompleted(message, myClient);
        }
        else if (tempOBJ is ThreeWayLeverCompletedMessage)
        {
            ThreeWayLeverCompletedMessage message = tempOBJ as ThreeWayLeverCompletedMessage;
            HandleThreeWayLeverTaskCompleted(message, myClient);
        }
        else if (tempOBJ is KeypadCompletedMessage)
        {
            KeypadCompletedMessage message = tempOBJ as KeypadCompletedMessage;
            HandleKeypadTaskCompleted(message, myClient);
        }
        else if(tempOBJ is LeaveServermessage)
        {
            LeaveServermessage message = tempOBJ as LeaveServermessage;
            HandleLeaveServerMessage(message, myClient);
        }
        else if(tempOBJ is LastPlayerTouchedMessage)
        {
            LastPlayerTouchedMessage message = tempOBJ as LastPlayerTouchedMessage;
            HandleLastPlayerTouchedMessage(message);
        }
        else if(tempOBJ is UpdateEnergyUserStatusResponse)
        {
            UpdateEnergyUserStatusResponse message = tempOBJ as UpdateEnergyUserStatusResponse;
            HandleEnergyUserStatus(message, myClient);
        }
        else if (tempOBJ is KeypadCodeUpdateRequest)
        {
            KeypadCodeUpdateRequest message = tempOBJ as KeypadCodeUpdateRequest;
            HandleKeypadUpdateRequest(message, myClient);
        }
        else if(tempOBJ is KeypadCodeOutcomeRequest)
        {
            KeypadCodeOutcomeRequest message = tempOBJ as KeypadCodeOutcomeRequest;
            HandleKeypadCodeOutcomeRequest(message);
        }
        else if (tempOBJ is KeypadValidationMessage)
        {
            KeypadValidationMessage message = tempOBJ as KeypadValidationMessage;
            HandleKeypadValidationMessage(message, myClient);
        }
    }

    //checks if the clients are still alive
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

    //sends a message that a player timed out
    void SendPlayerDisconnectMessages(string playerWhoDisconnected)
    {
        DateTime time = DateTime.Now;
        string messageToSend = playerWhoDisconnected + " timed out";

        TCPPacket outPacket = new TCPPacket();
        ChatRespons chatMessage = new ChatRespons(messageToSend, "Server", time.Hour, time.Minute, time.Second);
        outPacket.Write(chatMessage);
        SendTCPMessageToAllUsers(outPacket);
    }

    //refreshes the heartbeat of a client
    private void RefreshHeartbeat(MyClient pClient)
    {
        pClient.heartbeat = server.timeOutTime;
    }

    public override void UpdateRoom()
    {
        base.UpdateRoom();
    }

    //tells the user to make a map with the given seed and amountOfSectors
    void HandleSceneLoadedMessage(SceneLoadedMessage _message, MyClient client)
    {
        if(_message.sceneJoined == SceneLoadedMessage.scenes.game)
        {
            TCPPacket outPacket = new TCPPacket();
            MakeGameMapMessage makeGameMapMessage = new MakeGameMapMessage(worldSeed, amountOfSectors, roomsPerSector);
            outPacket.Write(makeGameMapMessage);
            SendTCPMessageToTargetUser(outPacket, client);
        }
    }

    //sends a udp message to all users with the new location when a player moves
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

    //sets the client endpoint end sendPort used for udp communication
    void HandleUpdateClientInfo(UpdateClientInfoMessage message, MyClient client)
    {
        client.endPoint = new IPEndPoint(message.ip, message.receivePort);
        client.sendPort = message.sendPort;
    }

    //checks if all players have made the map and are ready to make tasks/characters
    void HandleMapMadeMessage(MapMadeMessage message, MyClient client)
    {
        mapMadeMessages += 1;

        if (mapMadeMessages == clientsInRoom.Count)
        {
            MakeTasks();
            MakePlayerCharacters();
        }
    }

    //sends a message to all users that makes them an avatar and adds that avatar to all other clients
    void MakePlayerCharacters()
    {
        foreach (MyClient clientToSendTo in clientsInRoom)
        {
            foreach (MyClient clientToSend in clientsInRoom)
            {
                if (clientToSend != clientToSendTo)
                {
                    TCPPacket outpacket2 = new TCPPacket();
                    MakenewPlayerCharacterMessage makePlayerCharacterMessage2 = new MakenewPlayerCharacterMessage(false, clientToSend.playerID, clientToSend.playerName, clientToSend.playerColor);
                    outpacket2.Write(makePlayerCharacterMessage2);
                    SendTCPMessageToTargetUser(outpacket2, clientToSendTo);

                    Debug.Log(clientToSend.playerName + " " + clientToSend.playerColor);
                }
                else
                {
                    TCPPacket outpacket = new TCPPacket();
                    MakenewPlayerCharacterMessage makePlayerCharacterMessage = new MakenewPlayerCharacterMessage(true, clientToSend.playerID, clientToSend.playerName, clientToSend.playerColor);
                    outpacket.Write(makePlayerCharacterMessage);
                    SendTCPMessageToTargetUser(outpacket, clientToSendTo);
                }
            }
        }
    }

    //send a message to all clients to make tasks
    //only the server owner makes a taskManager
    void MakeTasks()
    {
        TCPPacket outpacket = new TCPPacket();
        PlaceWorldObjects makeTaskManagerMessage = new PlaceWorldObjects(false, maxErrors, tasksOfTypeToSpawn);
        outpacket.Write(makeTaskManagerMessage);
        SendTCPMessageToAllUsersExcept(outpacket, server.serverInfo.serverOwner);

        TCPPacket outpacket2 = new TCPPacket();
        PlaceWorldObjects makeTaskManagerMessage2 = new PlaceWorldObjects(true, maxErrors, tasksOfTypeToSpawn);
        outpacket2.Write(makeTaskManagerMessage2);
        SendTCPMessageToTargetUser(outpacket2, server.serverInfo.serverOwner);
    }

    //updates the station health
    void HandleUpdateStationHealthRequest(UpdateStationHealthRequest message, MyClient client)
    {
        TCPPacket outPacket = new TCPPacket();

        if(message.stationHealth > 0)
        {
            UpdateStationHealthResponse updateStationHealthResponse = new UpdateStationHealthResponse(message.stationHealth);
            outPacket.Write(updateStationHealthResponse);
            SendTCPMessageToAllUsersExcept(outPacket, client);
        }
        else
        {
            JoinRoomMessage joinRoomMessage = new JoinRoomMessage(JoinRoomMessage.rooms.endScreen);
            outPacket.Write(joinRoomMessage);
            SendTCPMessageToAllUsers(outPacket);

            server.serverInfo.finishedGamesTasksCompleted = tasksCompleted;
            server.AddRoomToMoveDictionary(this, server.endRoom);

            Reset();
        }
    }

    //when a taskLocation on the taskManager produces a task. add that task to all users except the client who generated it
    //under normal conditions the serverOwner is the only one that makes tasks
    void HandleAddKeypadTask(AddKeypadTaskMessage message, MyClient client)
    {
        TCPPacket outpacket = new TCPPacket();
        outpacket.Write(message);
        SendTCPMessageToAllUsers(outpacket);
    }

    void HandleAddTwoWayLeverTask(AddTwoWayLeverTask message, MyClient client)
    {
        TCPPacket outpacket = new TCPPacket();
        outpacket.Write(message);
        SendTCPMessageToAllUsers(outpacket);
    }

    void HandleAddThreeWayLeverTask(AddThreeWayLeverTask message, MyClient client)
    {
        TCPPacket outpacket = new TCPPacket();
        outpacket.Write(message);
        SendTCPMessageToAllUsers(outpacket);
    }

    //sends the new position of a lever to all other clients when a client pulls it
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

    //sends the completed task to all users except the user who completed it
    //under normal conditions the serverOwner is the only one that completes tasks
    void HandleTwoWayLeverTaskCompleted(TwoWayLeverCompletedMessage message, MyClient client)
    {
        TCPPacket outPacket = new TCPPacket();
        outPacket.Write(message);
        SendTCPMessageToAllUsers(outPacket);

        tasksCompleted += 1;
    }

    void HandleThreeWayLeverTaskCompleted(ThreeWayLeverCompletedMessage message, MyClient client)
    {
        TCPPacket outPacket = new TCPPacket();
        outPacket.Write(message);
        SendTCPMessageToAllUsers(outPacket);

        tasksCompleted += 1;
    }

    void HandleKeypadTaskCompleted(KeypadCompletedMessage message, MyClient client)
    {
        TCPPacket outPacket = new TCPPacket();
        outPacket.Write(message);
        SendTCPMessageToAllUsers(outPacket);

        tasksCompleted += 1;
    }

    void HandleKeypadValidationMessage(KeypadValidationMessage message, MyClient client)
    {
        TCPPacket outPacket = new TCPPacket();
        outPacket.Write(message);
        SendTCPMessageToTargetUser(outPacket, server.serverInfo.serverOwner);
    }

    //send new pos to all users
    void HandleUpdatePlayerCamPosition(UpdatePlayerCamPosition message, MyClient client)
    {
        UDPPacket outPacket = new UDPPacket();
        outPacket.Write(message);
        SendUDPMessageToAllUsers(outPacket);
    }

    void HandleUpdateEnergyCamPosition(UpdateEnergyCamPosition message, MyClient client)
    {
        UDPPacket outPacket = new UDPPacket();
        outPacket.Write(message);
        SendUDPMessageToAllUsersExcept(outPacket, client);
    }

    void HandleUpdateTaskCamPosition(UpdateTaskCamPosition message, MyClient client)
    {
        UDPPacket outPacket = new UDPPacket();
        outPacket.Write(message);
        SendUDPMessageToAllUsersExcept(outPacket, client);
    }

    void HandleLeaveServerMessage(LeaveServermessage message, MyClient myclient)
    {
        removeAndCloseMember(myclient);
        Debug.Log("deleting character");
    }

    void HandleUpdateRigidbodyPositionRequest(UpdateRigidbodyPositionRequest message, MyClient client)
    {
        UDPPacket outPacket = new UDPPacket();
        UpdateRigidbodyPositionResponse resposne = new UpdateRigidbodyPositionResponse(message.rigidbodyID, message.rigidbodyPosition, message.rigidbodyRotation);
        outPacket.Write(resposne);
        SendUDPMessageToAllUsersExcept(outPacket, client);
    }

    void HandleLastPlayerTouchedMessage(LastPlayerTouchedMessage message)
    {
        TCPPacket outPacket = new TCPPacket();
        outPacket.Write(message);
        SendTCPMessageToAllUsers(outPacket);
    }

    void HandleEnergyUserStatus(UpdateEnergyUserStatusResponse message, MyClient client)
    {
        TCPPacket outPacket = new TCPPacket();
        UpdateEnergyUserStatusResponse response = new UpdateEnergyUserStatusResponse(message.id, message.on);
        outPacket.Write(response);
        SendTCPMessageToAllUsersExcept(outPacket, client);
    }

    void HandleKeypadUpdateRequest(KeypadCodeUpdateRequest message, MyClient client)
    {
        TCPPacket outPacket = new TCPPacket();
        KeypadCodeUpdateResponse response = new KeypadCodeUpdateResponse(message.id, message.message);
        outPacket.Write(response);
        SendTCPMessageToAllUsersExcept(outPacket, client);
    }

    void HandleKeypadCodeOutcomeRequest(KeypadCodeOutcomeRequest message)
    {
        TCPPacket outPakcet = new TCPPacket();
        KeypadCodeOutcomeResponse response = new KeypadCodeOutcomeResponse(message.id, message.correct);
        outPakcet.Write(response);
        SendTCPMessageToAllUsers(outPakcet);
    }

    protected override void Reset()
    {
        mapMadeMessages = 0;
        worldSeed = Random.Range(1, 30);

        tasksCompleted = 0;
    }
}
