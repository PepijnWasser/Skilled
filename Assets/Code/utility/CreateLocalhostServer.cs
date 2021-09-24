using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class CreateLocalhostServer : MonoBehaviour
{
    [SerializeField] private int basePort = 55555;

    private LocalHostClientTCP localHostClient;

    public LocalHostServer localHostServerPrefab;
    public GameObject lobbyPrefab;


    private void Start()
    {
        localHostClient = GameObject.FindObjectOfType<LocalHostClientTCP>();
    }

    public void CreateLocalHostServer()
    {
        LocalHostServer server = Instantiate(localHostServerPrefab);
        server.Initialize(basePort);

        IPAddress _serverIP = Extensions.GetLocalIPAddress();
        if (localHostClient.ConnectToServer(_serverIP, server.GetServerTCPPort()))
        {
            Instantiate(lobbyPrefab);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(server.gameObject);
        }              
    }
}
