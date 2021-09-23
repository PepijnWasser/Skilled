using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System;

public class UDPSender : MonoBehaviour
{
    float secondCounter = 0;
    UdpClient client = new UdpClient();

    private void Start()
    {
        client = new UdpClient(33200);
        try
        {
            client.BeginReceive(new AsyncCallback(recv), null);
        }
        catch (Exception e)
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
        }
        else
        {
            Debug.Log("something else");
        }

        client.BeginReceive(new AsyncCallback(recv), null);
    }

    private void Update()
    {
        secondCounter += Time.deltaTime;
        if(secondCounter > 1)
        {
            secondCounter = 0;


            IPEndPoint RemoteIP = new IPEndPoint(IPAddress.Loopback, 20230);

            UDPPacket packet = new UDPPacket();
            UDPMessage message = new UDPMessage("are you there mate?");
            packet.Write(message);

            byte[] sendBytes = packet.GetBytes();

            client.Send(sendBytes, sendBytes.Length, RemoteIP);


        }
    }
}
