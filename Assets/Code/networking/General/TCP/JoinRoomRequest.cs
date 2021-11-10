using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JoinRoomRequest : ISerializable
{
    public enum rooms
    {
        lobby,
        game,
        endScreen
    }

    string roomToJoinS;
    public rooms roomToJoin;

    public void Deserialize(TCPPacket pPacket)
    {
        roomToJoinS = pPacket.ReadString();
        roomToJoin = (rooms)Enum.Parse(typeof(rooms), roomToJoinS);
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(roomToJoinS);
    }

    public JoinRoomRequest()
    {

    }

    public JoinRoomRequest(rooms _roomToJoin)
    {
        roomToJoinS = _roomToJoin.ToString();
    }
}
