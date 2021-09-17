using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;


public class Sender : MonoBehaviour
{
    float secondCOunter = 0;
    UdpClient client = new UdpClient();

    // Update is called once per frame
    void Update()
    {
        secondCOunter += Time.deltaTime;
        if(secondCOunter > 2)
        {
            secondCOunter = 0;
            Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");

            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.2.15"), 55555);
            client.Send(sendBytes, sendBytes.Length, ipEndPoint);
            Debug.Log("sending");
        }
    }
}
