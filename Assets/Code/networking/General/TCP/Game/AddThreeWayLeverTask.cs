using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddThreeWayLeverTask : ISerializable
{
    public ThreeWayLeverTask task;

    public void Deserialize(TCPPacket pPacket)
    {
        task = pPacket.ReadThreeWayLeverTask();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(task);
    }

    public AddThreeWayLeverTask()
    {

    }

    public AddThreeWayLeverTask(ThreeWayLeverTask _task)
    {
        task = _task;
    }
}
