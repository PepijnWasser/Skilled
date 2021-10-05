using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddKeypadTaskMessage : ISerializable
{
    public KeypadTask task;

    public void Deserialize(TCPPacket pPacket)
    {
        task = pPacket.ReadKeypadTask();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(task);
    }

    public AddKeypadTaskMessage()
    {

    }

    public AddKeypadTaskMessage(KeypadTask _task)
    {
        task = _task;
    }
}
