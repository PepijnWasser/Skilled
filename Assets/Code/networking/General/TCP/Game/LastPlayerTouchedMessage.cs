using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPlayerTouchedMessage : ISerializable
{
    public int playerID;
    public int rigidbodyID;

    public void Deserialize(TCPPacket pPacket)
    {
        playerID = pPacket.ReadInt();
        rigidbodyID = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(playerID);
        pPacket.Write(rigidbodyID);
    }

    public LastPlayerTouchedMessage()
    {

    }

    public LastPlayerTouchedMessage(int _playerID, int _rigidbodyID)
    {
        playerID = _playerID;
        rigidbodyID = _rigidbodyID;
    }
}
