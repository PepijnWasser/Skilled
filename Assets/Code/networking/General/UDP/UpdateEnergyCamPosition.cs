using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEnergyCamPosition : USerializable
{
    public Vector3 cameraPosition;
    public float zoom;

    public void Deserialize(UDPPacket pPacket)
    {
        cameraPosition = pPacket.ReadVector3();
        zoom = pPacket.ReadFloat();
    }

    public void Serialize(UDPPacket pPacket)
    {
        pPacket.Write(cameraPosition);
        pPacket.Write(zoom);
    }

    public UpdateEnergyCamPosition()
    {

    }

    public UpdateEnergyCamPosition(Vector3 newPos, float newzoom)
    {
        cameraPosition = newPos;
        zoom = newzoom;
    }
}
