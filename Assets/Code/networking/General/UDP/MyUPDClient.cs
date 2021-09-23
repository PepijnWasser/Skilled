using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class MyUPDClient : MonoBehaviour
{
    /*
    float secondCOunter = 0;
    UdpClient client = new UdpClient(33445);

    
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
        IPEndPoint RemoteEndPoint = new IPEndPoint(IPAddress.Any, 32323);
        byte[] received = NetworkUtils.Read(client.EndReceive(res, ref RemoteEndPoint));
        Debug.Log("data from server received");

        UDPPacket inPacket = new UDPPacket(received);
        var tempOBJ = inPacket.ReadObject();

        if (tempOBJ is UDPMessage)
        {
            UDPMessage message = tempOBJ as UDPMessage;
            Debug.Log(message.message);
        }


        client.BeginReceive(new AsyncCallback(Recv), null);
    }
    

    void Update()
    {
        byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");

        secondCOunter += Time.deltaTime;
        if (secondCOunter > 2)
        {
            secondCOunter = 0;
            UDPPacket packet = new UDPPacket();
            UDPMessage message = new UDPMessage("hi");
            packet.Write(message);
            sendBytes = packet.GetBytes();


            //sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.2.1"), 32323);
            client.Send(sendBytes, sendBytes.Length, ipEndPoint);
            Debug.Log("sending");
        }
    }
    */
}
