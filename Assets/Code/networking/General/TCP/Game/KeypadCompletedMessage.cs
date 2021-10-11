using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadCompletedMessage : ISerializable
{
    public KeypadTask task;
    public int keypadID;

    public void Deserialize(TCPPacket pPacket)
    {
        task = pPacket.ReadKeypadTask();
        keypadID = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(task);
        pPacket.Write(keypadID);
    }

    public KeypadCompletedMessage()
    {

    }

    public KeypadCompletedMessage(KeypadTask _task, int _keypadID)
    {
        task = _task;
        keypadID = _keypadID;
    }
}
