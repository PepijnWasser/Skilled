using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovePlayerBarMessage : ISerializable
{
    public int playerID;

    public void Deserialize(Packet pPacket)
    {
        playerID = pPacket.ReadInt();
    }

    public void Serialize(Packet pPacket)
    {
        pPacket.Write(playerID);
    }

    public RemovePlayerBarMessage()
    {

    }

    public RemovePlayerBarMessage(int _playerID)
    {
        playerID = _playerID;
    }
}
