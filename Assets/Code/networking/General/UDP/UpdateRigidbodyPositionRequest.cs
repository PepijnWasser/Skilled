using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRigidbodyPositionRequest : USerializable
{ 
    public Vector3 rigidbodyPosition;
    public Vector3 rigidbodyRotation;
    public int rigidbodyID;

    public void Deserialize(UDPPacket pPacket)
    {
        rigidbodyPosition = pPacket.ReadVector3();
        rigidbodyRotation = pPacket.ReadVector3();
        rigidbodyID = pPacket.ReadInt();
    }

    public void Serialize(UDPPacket pPacket)
    {
        pPacket.Write(rigidbodyPosition);
        pPacket.Write(rigidbodyRotation);
        pPacket.Write(rigidbodyID);
    }

    public UpdateRigidbodyPositionRequest()
    {

    }

    public UpdateRigidbodyPositionRequest (int _ID, Vector3 _playerPosition, Vector3 _playerRotation)
    {
        rigidbodyPosition = _playerPosition;
        rigidbodyRotation = _playerRotation;
        rigidbodyID = _ID;
    }
}
