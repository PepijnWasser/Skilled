using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerPositionUDP : USerializable
{
    public Vector3 playerPosition;
    public Vector3 playerRotation;
    public int playerID;

    public void Deserialize(UDPPacket pPacket)
    {
        playerPosition = pPacket.ReadVector3();
        playerRotation = pPacket.ReadVector3();
        playerID = pPacket.ReadInt();
    }

    public void Serialize(UDPPacket pPacket)
    {
        pPacket.Write(playerPosition);
        pPacket.Write(playerRotation);
        pPacket.Write(playerID);
    }

    public UpdatePlayerPositionUDP()
    {

    }

    public UpdatePlayerPositionUDP(Vector3 _playerPosition, Vector3 _playerRotation)
    {
        playerPosition = _playerPosition;
        playerRotation = _playerRotation;
    }
    public UpdatePlayerPositionUDP(Vector3 _playerPosition, Vector3 _playerRotation, int _playerID)
    {
        playerPosition = _playerPosition;
        playerRotation = _playerRotation;
        playerID = _playerID;
    }
}
