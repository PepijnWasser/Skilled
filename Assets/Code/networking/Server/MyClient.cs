using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using UnityEngine;

public class MyClient
{
    public float heartbeat;
    public TcpClient TcpClient;
    public int playerID;
    public string playerName;
    public Vector3 playerPosition;


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
        TcpClient = _TcpClient;
        playerColor = _playerColor;
        playerID = _playerID;
        playerName = _playerName;
    }
}
