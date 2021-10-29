using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class PlayerInfo
{
	public static int playerID;
	public static string playerName;
	public static int udpReceivePort;
	public static int udpSendPort;
	public static IPAddress ip;

    private void Start()
    {
		ip = Extensions.GetLocalIPAddress();
    }
}
