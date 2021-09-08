using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class CreateLocalhostServer : MonoBehaviour
{
    [SerializeField] private string _serverString = "localhost";
    [SerializeField] private string _serverIPString = "86.86.80.67";
    private IPAddress _serverIP;
    private IPEndPoint _serverEndPoint;
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

        _serverIP = IPAddress.Parse(_serverIPString);
        _serverEndPoint = new IPEndPoint(_serverIP, server.GetServerPort());
        
        if (localHostClient.ConnectToServer(_serverString, server.GetServerPort()))
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
