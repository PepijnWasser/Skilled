using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using UnityEngine;
using System.Net;

using Random = UnityEngine.Random;


public class MyClient
{
    public float heartbeat;
    public TcpClient tcpClient;

    public IPEndPoint endPoint;
    public int sendPort;

    public int playerID;
    public string playerName;


    public enum colors
    {
        red,
        blue,
        green,
        yellow
    }

    public colors playerColor;


    public MyClient(TcpClient _TcpClient, float _heartbeat, colors _playerColor, int _playerID, string _playerName)
    {
        heartbeat = _heartbeat;
        tcpClient = _TcpClient;
        playerColor = _playerColor;
        playerID = _playerID;
        playerName = _playerName;
    }
}
