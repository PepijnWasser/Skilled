using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateColorMessage : ISerializable
{
    public string color = "red";

    public void Serialize(Packet packet)
    {
        packet.Write(color);
    }

    public void Deserialize(Packet packet)
    {
        color = packet.ReadString();
    }

    public UpdateColorMessage()
    {

    }

    public UpdateColorMessage(MyClient.colors _color)
    {
        color = _color.ToString();
    }
}