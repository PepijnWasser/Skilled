using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceWorldObjects : ISerializable
{
    public bool playerIsLeader;
    public int maxErrors;
    public int tasksOfTypeToSpawn;

    public void Deserialize(TCPPacket pPacket)
    {
        playerIsLeader = pPacket.ReadBool();
        maxErrors = pPacket.ReadInt();
        tasksOfTypeToSpawn = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(playerIsLeader);
        pPacket.Write(maxErrors);
        pPacket.Write(tasksOfTypeToSpawn);
    }

    public PlaceWorldObjects()
    {

    }

    public PlaceWorldObjects(bool _playerIsLeader, int _maxErrors, int _tasksOfTypeToSpawn)
    {
        playerIsLeader = _playerIsLeader;
        maxErrors = _maxErrors;
        tasksOfTypeToSpawn = _tasksOfTypeToSpawn;
    }
}
