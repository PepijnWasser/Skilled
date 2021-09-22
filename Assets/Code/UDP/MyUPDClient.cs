using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class MyUPDClient : MonoBehaviour
{
    float secondCOunter = 0;
    UdpClient client = new UdpClient();

    // Update is called once per frame
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

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.2.15"), 33339);
            client.Send(sendBytes, sendBytes.Length, ipEndPoint);
            Debug.Log("sending");
        }
    }
}
