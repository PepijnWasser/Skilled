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
        public int playerID;


        public MyClient(TcpClient _TcpClient, int _playerID, float _heartbeat)
        {
            heartbeat = _heartbeat;
            TcpClient = _TcpClient;
            playerID = _playerID;
        }
    
}
