using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerCountMessage : ISerializable
{

    public int playerCount;

    public void Serialize(Packet packet)
    {
        packet.Write(playerCount);
    }

    public void Deserialize(Packet packet)
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
