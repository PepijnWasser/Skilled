using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeNewPlayerBarMessage : ISerializable
{
    public int playerID;
    public string playerColor;
    public string playerName;
    public bool isPlayer = true;

    public void Deserialize(Packet pPacket)
    {
        playerID = pPacket.ReadInt();
        playerColor = pPacket.ReadString();
        playerName = pPacket.ReadString();
        isPlayer = pPacket.ReadBool();
    }

    public void Serialize(Packet pPacket)
    {
        pPacket.Write(playerID);
        pPacket.Write(playerColor);
        pPacket.Write(playerName);
        pPacket.Write(isPlayer);
    }

    public MakeNewPlayerBarMessage()
    {

    }

    public MakeNewPlayerBarMessage(int _playerID, MyClient.colors _playerColor, string _playerName, bool _isPlayer)
    {
        playerID = _playerID;
        playerColor = _playerColor.ToString();
        playerName = _playerName;
        isPlayer = _isPlayer;
    }
}
