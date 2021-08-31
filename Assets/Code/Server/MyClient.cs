using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

    public class MyClient
    {
        public int heartbeat;
        public TcpClient TcpClient;
        public int playerID;


        public MyClient(TcpClient _TcpClient, int _playerID)
        {
            heartbeat = 100;
            TcpClient = _TcpClient;
            playerID = _playerID;
        }
    
}
