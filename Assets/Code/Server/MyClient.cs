using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

    public class MyClient
    {
    public float heartbeat;
    public TcpClient TcpClient;
    public enum colors
    {
        red,
        blue,
        green,
        yellow
    }

    public colors playerColor;


    public MyClient(TcpClient _TcpClient, float _heartbeat, colors _playerColor)
    {
        heartbeat = _heartbeat;
        TcpClient = _TcpClient;
        playerColor = _playerColor;
    }
}
