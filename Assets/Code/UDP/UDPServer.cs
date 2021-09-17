using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

public class UDPServer : MonoBehaviour
{
    static UdpClient client = new UdpClient(33332);
    string data = "";

    void Start()
    {
        try
        {
            client.BeginReceive(new AsyncCallback(Recv), null);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    void Recv(IAsyncResult res)
    {
        IPEndPoint RemoteEndPoint = new IPEndPoint(IPAddress.Any, 33332);
        byte[] received = NetworkUtils.Read(client.EndReceive(res, ref RemoteEndPoint));
        Debug.Log("data received");

        UDPPacket inPacket = new UDPPacket(received);
        var tempOBJ = inPacket.ReadObject();

        if(tempOBJ is UDPMessage)
        {
            UDPMessage message = tempOBJ as UDPMessage;
            Debug.Log(message.message);
        }
        //data = Encoding.UTF8.GetString(received);
        //Debug.Log(data + " this message was send from " + RemoteEndPoint.Address + " on port " + RemoteEndPoint.Port);

        client.BeginReceive(new AsyncCallback(Recv), null);
    }
}
