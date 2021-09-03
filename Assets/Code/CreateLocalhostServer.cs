using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLocalhostServer : MonoBehaviour
{
    [SerializeField] private string _server = "localhost";
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
        string serverOwner = GameObject.FindObjectOfType<LocalHostClient>().playerName;
        server.Initialize(basePort, serverOwner);

        if (localHostClient.ConnectToServer(_server, server.GetServerPort()))
        {
            Instantiate(lobbyPrefab);
            localHostClient.SendPlayerNameRequest();
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(server.gameObject);
        }
    }
}
