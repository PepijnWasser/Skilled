using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateKeypadRequest : ISerializable
{
    public string code;
    public int keypadID;

    public void Deserialize(TCPPacket pPacket)
    {
        code = pPacket.ReadString();
        keypadID = pPacket.ReadInt();
    }

    public void Serialize(TCPPacket pPacket)
    {
        pPacket.Write(code);
        pPacket.Write(keypadID);
    }

    public UpdateKeypadRequest()
    {

    }

    public UpdateKeypadRequest(string _code, int _keypadID)
    {
        code = _code;
        keypadID = _keypadID;
    }
}
