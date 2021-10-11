using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerNameRespons : ISerializable
{
    public string playerName;
    public int playerID;

    public void Deserialize(TCPPacket pPacket)
    {
        playerName = pPacket.ReadString();
        playerID = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(playerName);
        pPacket.Write(playerID);
    }

    public UpdatePlayerNameRespons()
    {

    }

    public UpdatePlayerNameRespons(string _playerName, int _playerID)
    {
        playerName = _playerName;
        playerID = _playerID;
    }
}
