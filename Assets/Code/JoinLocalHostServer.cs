using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class JoinLocalHostServer : MonoBehaviour
{
    [SerializeField] private string _serverString = "localhost";
    [SerializeField] private string _serverIPString = "86.86.80.67";
    [SerializeField] private int basePort = 55555;
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
