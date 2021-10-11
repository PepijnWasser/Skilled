using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerCamPosition : USerializable
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

    public UpdatePlayerCamPosition()
    {

    }

    public UpdatePlayerCamPosition(Vector3 newPos)
    {
        cameraPosition = newPos;
    }
}
