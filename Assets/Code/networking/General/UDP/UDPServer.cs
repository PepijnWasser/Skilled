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
    private UdpClient client = new UdpClient(1111);
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
        IPEndPoint RemoteEndPoint = new IPEndPoint(IPAddress.Any, 60240);
        byte[] received = client.EndReceive(res, ref RemoteEndPoint); // NetworkUtils.Read(client.EndReceive(res, ref RemoteEndPoint));

        data = Encoding.UTF8.GetString(received);
        Debug.Log(data);
        /*
        UDPPacket inPacket = new UDPPacket(received);
        var tempOBJ = inPacket.ReadObject();

        if(tempOBJ is UDPMessage)
        {
            UDPMessage message = tempOBJ as UDPMessage;
            Debug.Log(message.message);
            //Debug.Log("address: " + RemoteEndPoint.Address + " port: " + RemoteEndPoint.Port);
            
            byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");
            UDPPacket packet2 = new UDPPacket();
            UDPMessage message2 = new UDPMessage("HELLO");
            packet2.Write(message2);
            sendBytes = packet2.GetBytes();

            client.Send(sendBytes, sendBytes.Length, RemoteEndPoint);       
        }
        */

        client.BeginReceive(new AsyncCallback(Recv), null);
    }
}
