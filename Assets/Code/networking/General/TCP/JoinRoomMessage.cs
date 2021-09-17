using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JoinRoomMessage : ISerializable
{
    public enum rooms
    {
        mainMenu,
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

    public JoinRoomMessage()
    {

    }

    public JoinRoomMessage(rooms _roomToJoin)
    {
        roomToJoinS = _roomToJoin.ToString();
    }
}
