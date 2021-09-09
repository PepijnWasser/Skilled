using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class CreateLocalhostServer : MonoBehaviour
{
    [SerializeField] private string _serverIPString = "192.168.2.10";
    private IPAddress _serverIP;
    [SerializeField] private int basePort = 20017;

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
}
