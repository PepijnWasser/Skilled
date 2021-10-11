using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddThreeWayLeverTask : ISerializable
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

    public AddThreeWayLeverTask()
    {

    }

    public AddThreeWayLeverTask(ThreeWayLeverTask _task, int _levereID)
    {
        task = _task;
        leverID = _levereID;
    }
}
