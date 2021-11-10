using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskscompletedMessage : ISerializable
{
    public int amountOfTasksCompleted;

    public void Deserialize(TCPPacket pPacket)
    {
        amountOfTasksCompleted = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(amountOfTasksCompleted);
    }

    public TaskscompletedMessage()
    {

    }

    public TaskscompletedMessage(int _amountOfTasksCompleted)
    {
        amountOfTasksCompleted = _amountOfTasksCompleted;
    }
}
