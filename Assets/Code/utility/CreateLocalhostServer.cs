using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class CreateLocalhostServer : MonoBehaviour
{
    [SerializeField] private int basePort = 55555;

    private LocalHostClient localHostClient;

    public LocalHostServer localHostServerPrefab;
    public GameObject lobbyPrefab;


    private void Start()
    {
        localHostClient = GameObject.FindObjectOfType<LocalHostClient>();
    }

    public void CreateLocalHostServer()
    {
        LocalHostServer server = Instantiate(localHostServerPrefab);
        server.Initialize(basePort);

        IPAddress _serverIP = IPAddress.Parse(GetIPAddress());
        if (localHostClient.ConnectToServer(_serverIP, server.GetServerPort()))
        {
            Instantiate(lobbyPrefab);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(server.gameObject);
        }              
    }

    string GetIPAddress()
    {
        IPHostEntry host;
        string localIP = "0.0.0.0";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
}
