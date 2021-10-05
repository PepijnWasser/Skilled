using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerCountMessage : ISerializable
{

    public int playerCount;

    public void Serialize(TCPPacket packet)
    {
        packet.Write(playerCount);
    }

    public void Deserialize(TCPPacket packet)
    {
        playerCount = packet.ReadInt();
    }

    public UpdatePlayerCountMessage()
    {

    }

    public UpdatePlayerCountMessage(int _playerCount)
    {
        playerCount = _playerCount;
    }
}
