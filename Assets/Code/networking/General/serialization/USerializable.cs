using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface USerializable
{
    /**
  * Write all the data for 'this' object into the Packet
  */
    void Serialize(UDPPacket pPacket);

    /**
     * Read all the data for 'this' object from the Packet
     */
    void Deserialize(UDPPacket pPacket);
}
