using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeGameMapMessage : ISerializable
{
    public int worldSeed;
    public int amountOfSectors;
    public int roomsPerSector;

    public void Deserialize(TCPPacket pPacket)
    {
        worldSeed = pPacket.ReadInt();
        amountOfSectors = pPacket.ReadInt();
        roomsPerSector = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(worldSeed);
        pPacket.Write(amountOfSectors);
        pPacket.Write(roomsPerSector);
    }

    public MakeGameMapMessage()
    {

    }

    public MakeGameMapMessage(int _worldSeed, int _amountOfSectors, int _roomsPerSector)
    {
        worldSeed = _worldSeed;
        amountOfSectors = _amountOfSectors;
        roomsPerSector = _roomsPerSector;
    }
}
