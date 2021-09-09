using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoom : Room
{
    public override void handleNetworkMessageFromUser(ISerializable pMessage, MyClient pSender)
    {
       // throw new System.NotImplementedException();
    }

    public override void UpdateRoom()
    {
        base.UpdateRoom();
        Debug.Log(GetMembers().Count);
    }
}
