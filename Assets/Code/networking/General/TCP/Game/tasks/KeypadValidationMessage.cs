using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadValidationMessage : ISerializable
{
    public int keypadID;
    public string code;

    public void Deserialize(TCPPacket pPacket)
    {
        keypadID = pPacket.ReadInt();
        code = pPacket.ReadString();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(keypadID);
        pPacket.Write(code);
    }

    public KeypadValidationMessage()
    {

    }

    public KeypadValidationMessage(string _code, int _keypadID)
    {
        code = _code;
        keypadID = _keypadID;
    }
}
