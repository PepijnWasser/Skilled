using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class JoinLocalHostServer : MonoBehaviour
{
    [SerializeField] private string _serverIPString = "192.168.2.10";
    [SerializeField] private int basePort = 20017;
    private IPAddress _serverIP;

    private LocalHostClient localHostClient;

    public GameObject lobbyPrefab;

    private void Start()
    {
        localHostClient = GameObject.FindObjectOfType<LocalHostClient>();
    }

    public void TryConnectingClient()
    {
        _serverIP = IPAddress.Parse(_serverIPString);

        if (localHostClient.ConnectToServer(_serverIP, basePort))
        {
            Destroy(this.gameObject);
            Instantiate(lobbyPrefab);
        }
    }
}
