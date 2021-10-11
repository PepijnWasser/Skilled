using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovePlayerBarMessage : ISerializable
{
    public int playerID;

    public void Deserialize(TCPPacket pPacket)
    {
        playerID = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
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
