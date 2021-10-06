using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateKeypadStatusMessage : ISerializable
{
    public int keypadID;
    public bool isInUse;

    public void Deserialize(TCPPacket pPacket)
    {
        keypadID = pPacket.ReadInt();
        isInUse = pPacket.ReadBool();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(keypadID);
        pPacket.Write(isInUse);
    }

    public UpdateKeypadStatusMessage()
    {

    }

    public UpdateKeypadStatusMessage(int _keypadID, bool _isInUse)
    {
        isInUse = _isInUse;
        keypadID = _keypadID;
    }
}
