using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateThreeWayLeverPositionMessage : ISerializable
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

    public UpdateThreeWayLeverPositionMessage()
    {

    }

    public UpdateThreeWayLeverPositionMessage(int _leverPosition, int _leverID)
    {
        leverPosition = _leverPosition;
        leverID = _leverID;
    }
}
