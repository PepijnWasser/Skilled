using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayLeverCompletedMessage : ISerializable
{
    public TwoWayLeverTask task;
    public int leverID;

    public void Deserialize(TCPPacket pPacket)
    {
        task = pPacket.ReadTwoWayLeverTask();
        leverID = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(task);
        pPacket.Write(leverID);
    }

    public TwoWayLeverCompletedMessage()
    {

    }

    public TwoWayLeverCompletedMessage(TwoWayLeverTask _task, int _leverID)
    {
        task = _task;
        leverID = _leverID;
    }
}
