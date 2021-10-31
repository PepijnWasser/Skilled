using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoom : Room
{
    public override void HandleTCPNetworkMessageFromUser(ISerializable pMessage, MyClient pSender)
    {
        //throw new System.NotImplementedException();
    }

    public override void HandleUDPNetworkMessageFromUser(USerializable pMessage, MyClient pSender)
    {
       // throw new System.NotImplementedException();
    }
}
