using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateThreeWayLeverPositionRequest : ISerializable
{
    public int leverPosition;
    public int leverID;

    public void Deserialize(TCPPacket pPacket)
    {
        leverPosition = pPacket.ReadInt();
        leverID = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(leverPosition);
        pPacket.Write(leverID);
    }

    public UpdateThreeWayLeverPositionRequest()
    {

    }

    public UpdateThreeWayLeverPositionRequest(int _leverPosition, int _leverID)
    {
        leverPosition = _leverPosition;
        leverID = _leverID;
    }
}
