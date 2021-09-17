using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

public class Receiver : MonoBehaviour
{
    static UdpClient client = new UdpClient(44444);
    string data = "";

    void Start()
    {

            client.BeginReceive(new AsyncCallback(Recv), null);

    }

    void Recv(IAsyncResult res)
    {
        IPEndPoint RemoteEndPoint = new IPEndPoint(IPAddress.Any, 44444);
        byte[] received = client.EndReceive(res, ref RemoteEndPoint);
        data = Encoding.UTF8.GetString(received);
        Debug.Log(data + " this message was send from " + RemoteEndPoint.Address + " on port " + RemoteEndPoint.Port);

        client.BeginReceive(new AsyncCallback(Recv), null);
    }
}
