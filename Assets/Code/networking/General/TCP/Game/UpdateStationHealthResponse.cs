using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateStationHealthResponse : ISerializable
{
    public int stationHealth;

    public void Deserialize(TCPPacket pPacket)
    {
        stationHealth = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(stationHealth);
    }

    public UpdateStationHealthResponse()
    {

    }

    public UpdateStationHealthResponse(int _stationHealth)
    {
        stationHealth = _stationHealth;
    }
}
