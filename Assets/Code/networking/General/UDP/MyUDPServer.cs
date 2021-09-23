using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class MyUDPServer : MonoBehaviour
{
    UdpClient client = new UdpClient();

    void Start()
    {
        client = new UdpClient(20230);

        try
        {
            client.BeginReceive(new AsyncCallback(recv), null);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    void recv(IAsyncResult res)
    {
        IPEndPoint RemoteIP = new IPEndPoint(IPAddress.Any, 60240);
        byte[] received = client.EndReceive(res, ref RemoteIP);

        UDPPacket packet = new UDPPacket(received);
        var TempOBJ = packet.ReadObject();

        if (TempOBJ is UDPMessage)
        {
            UDPMessage message = TempOBJ as UDPMessage;
            Debug.Log(message.message);
            SendResponse();
        }
        else
        {
            Debug.Log("something else");
        }

        client.BeginReceive(new AsyncCallback(recv), null);
    }

    void SendResponse()
    {
        IPEndPoint RemoteIP = new IPEndPoint(IPAddress.Parse("192.168.2.1"), 33200);

        UDPPacket packet = new UDPPacket();
        UDPMessage message = new UDPMessage("indeed");
        packet.Write(message);

        byte[] sendBytes = packet.GetBytes();

        client.Send(sendBytes, sendBytes.Length, RemoteIP);
    }
}
