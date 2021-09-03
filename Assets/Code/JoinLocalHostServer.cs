using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinLocalHostServer : MonoBehaviour
{
    [SerializeField] private string _server = "localhost";
    [SerializeField] private int basePort = 55555;

    private LocalHostClient localHostClient;

    public GameObject lobbyPrefab;

    private void Start()
    {
        localHostClient = GameObject.FindObjectOfType<LocalHostClient>();
    }

    public void TryConnectingClient(int port)
    {
        if (localHostClient.ConnectToServer(_server, port))
        {
            Destroy(this.gameObject);
            localHostClient.SendPlayerNameRequest();
            Instantiate(lobbyPrefab);
        }
    }
}
