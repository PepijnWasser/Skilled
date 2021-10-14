using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerCamPosition : USerializable
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

    public UpdatePlayerCamPosition()
    {

    }

    public UpdatePlayerCamPosition(Vector3 newPos, float newZoom)
    {
        cameraPosition = newPos;
        zoom = newZoom;
    }
}
