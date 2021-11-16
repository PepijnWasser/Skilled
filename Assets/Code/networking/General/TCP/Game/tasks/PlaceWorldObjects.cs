using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceWorldObjects : ISerializable
{
    public bool playerIsLeader;

    public void Deserialize(TCPPacket pPacket)
    {
        playerIsLeader = pPacket.ReadBool();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(playerIsLeader);
    }

    public PlaceWorldObjects()
    {

    }

    public PlaceWorldObjects(bool _playerIsLeader)
    {
        playerIsLeader = _playerIsLeader;
    }
}
