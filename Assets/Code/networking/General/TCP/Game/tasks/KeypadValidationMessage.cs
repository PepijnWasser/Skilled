using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadValidationMessage : ISerializable
{
    public int keypadID;
    public int code;

    public void Deserialize(TCPPacket pPacket)
    {
        keypadID = pPacket.ReadInt();
        code = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(keypadID);
        pPacket.Write(code);
    }

    public KeypadValidationMessage()
    {

    }

    public KeypadValidationMessage(int _code, int _keypadID)
    {
        code = _code;
        keypadID = _keypadID;
    }
}
