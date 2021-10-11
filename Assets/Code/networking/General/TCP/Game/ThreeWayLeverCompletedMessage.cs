using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeWayLeverCompletedMessage : ISerializable
{
    public ThreeWayLeverTask task;
    public int leverID;

    public void Deserialize(TCPPacket pPacket)
    {
        task = pPacket.ReadThreeWayLeverTask();
        leverID = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(task);
        pPacket.Write(leverID);
    }

    public ThreeWayLeverCompletedMessage()
    {

    }

    public ThreeWayLeverCompletedMessage(ThreeWayLeverTask _task, int _leverID)
    {
        task = _task;
        leverID = _leverID;
    }
}
