using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeGameMapMessage : ISerializable
{
    public int worldSeed;
    public int amountOfSectors;

    public void Deserialize(Packet pPacket)
    {
        worldSeed = pPacket.ReadInt();
        amountOfSectors = pPacket.ReadInt();
    }

    public void Serialize(Packet pPacket)
    {
        pPacket.Write(worldSeed);
        pPacket.Write(amountOfSectors);
    }

    public MakeGameMapMessage()
    {

    }

    public MakeGameMapMessage(int _worldSeed, int _amountOfSectors)
    {
        worldSeed = _worldSeed;
        amountOfSectors = _amountOfSectors;
    }
}
