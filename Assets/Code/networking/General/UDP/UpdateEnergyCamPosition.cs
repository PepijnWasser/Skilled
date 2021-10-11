using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEnergyCamPosition : USerializable
{
    public Vector3 cameraPosition;

    public void Deserialize(UDPPacket pPacket)
    {
        cameraPosition = pPacket.ReadVector3();
    }

    public void Serialize(UDPPacket pPacket)
    {
        pPacket.Write(cameraPosition);
    }

    public UpdateEnergyCamPosition()
    {

    }

    public UpdateEnergyCamPosition(Vector3 newPos)
    {
        cameraPosition = newPos;
    }
}
