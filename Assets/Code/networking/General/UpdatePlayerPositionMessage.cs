using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerPositionMessage : ISerializable
{
    public Vector3 playerPosition;
    public int playerID;

    public void Deserialize(Packet pPacket)
    {
        playerPosition = pPacket.ReadVector3();
    }

    public void Serialize(Packet pPacket)
    {
        pPacket.Write(playerPosition);
    }

    public UpdatePlayerPositionMessage()
    {

    }

    public UpdatePlayerPositionMessage(Vector3 _playerPosition)
    {
        playerPosition = _playerPosition;
    }
    public UpdatePlayerPositionMessage(Vector3 _playerPosition, int _playerID)
    {
        playerPosition = _playerPosition;
        playerID = _playerID;
    }

}
