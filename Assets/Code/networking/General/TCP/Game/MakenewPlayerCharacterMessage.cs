using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MakenewPlayerCharacterMessage : ISerializable
{
    public bool isPlayer = true;
    public int playerID;
    public string playerName;
    public string colorS;

    public void Deserialize(TCPPacket pPacket)
    {
        isPlayer = pPacket.ReadBool();
        playerID = pPacket.ReadInt();
        playerName = pPacket.ReadString();
        colorS = pPacket.ReadString();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(isPlayer);
        pPacket.Write(playerID);
        pPacket.Write(playerName);
        pPacket.Write(colorS);
    }

    public MakenewPlayerCharacterMessage()
    {

    }

    public MakenewPlayerCharacterMessage(bool _isPlayer, int _playerID, string _playerName, MyClient.colors _color)
    {
        isPlayer = _isPlayer;
        playerID = _playerID;
        playerName = _playerName;
        colorS = _color.ToString();
    }
}
