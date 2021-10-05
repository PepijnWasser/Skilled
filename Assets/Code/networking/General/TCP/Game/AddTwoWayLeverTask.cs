using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTwoWayLeverTask : ISerializable
{
    public TwoWayLeverTask task;

    public void Deserialize(TCPPacket pPacket)
    {
        task = pPacket.ReadTwoWayLeverTask();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(task);
    }

    public AddTwoWayLeverTask()
    {

    }

    public AddTwoWayLeverTask(TwoWayLeverTask _task)
    {
        task = _task;
    }
}
