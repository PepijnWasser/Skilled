using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoom : Room
{
    public override void HandleTCPNetworkMessageFromUser(ISerializable tempOBJ, MyClient myClient)
    {
        if (tempOBJ is HeartBeat)
        {
            RefreshHeartbeat(myClient);
        }
        else if(tempOBJ is SceneLoadedMessage)
        {
            SceneLoadedMessage message = tempOBJ as SceneLoadedMessage;
            HandleSceneLoadedMessage(message, myClient);
        }
        else if(tempOBJ is JoinRoomRequest)
        {
            JoinRoomRequest request = tempOBJ as JoinRoomRequest;
            HandleJoinRoomRequest(request, myClient);
        }
    }

    public override void HandleUDPNetworkMessageFromUser(USerializable pMessage, MyClient pSender)
    {

    }

    private void RefreshHeartbeat(MyClient myClient)
    {
        myClient.heartbeat = server.timeOutTime;
    }

    private void HandleSceneLoadedMessage(SceneLoadedMessage message, MyClient myClient)
    {
        if(message.sceneJoined == SceneLoadedMessage.scenes.endScreen)
        {
            TCPPacket outPacket = new TCPPacket();
            TaskscompletedMessage taskCompletedMessage = new TaskscompletedMessage(5);
            outPacket.Write(taskCompletedMessage);
            SendTCPMessageToTargetUser(outPacket, myClient);
        }
    }

    void HandleJoinRoomRequest(JoinRoomRequest request, MyClient myClient)
    {
        if(request.roomToJoin == JoinRoomRequest.rooms.lobby)
        {
            TCPPacket outPacket = new TCPPacket();
            JoinRoomMessage message = new JoinRoomMessage(JoinRoomMessage.rooms.lobby);
            outPacket.Write(message);
            SendTCPMessageToTargetUser(outPacket, myClient);

            server.timeOutTime = 6f;

            server.AddPlayerToMoveDictionary(myClient, server.lobbyRoom);
            //server.AddRoomToMoveDictionary(this, server.lobbyRoom);
        }
    }
}
