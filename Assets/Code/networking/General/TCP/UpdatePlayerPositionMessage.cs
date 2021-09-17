using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerPositionMessage : ISerializable
{
    public Vector3 playerPosition;
    public Vector3 playerRotation;
    public int playerID;

    public void Deserialize(TCPPacket pPacket)
    {
        playerPosition = pPacket.ReadVector3();
        playerRotation = pPacket.ReadVector3();
        playerID = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(playerPosition);
        pPacket.Write(playerRotation);
        pPacket.Write(playerID);
    }

    public UpdatePlayerPositionMessage()
    {

    }

    public UpdatePlayerPositionMessage(Vector3 _playerPosition)
    {
        playerPosition = _playerPosition;
    }
    public UpdatePlayerPositionMessage(Vector3 _playerPosition, Vector3 _playerRotation, int _playerID)
    {
        playerPosition = _playerPosition;
        playerRotation = _playerRotation;
        playerID = _playerID;
    }

}
