using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

public class JoinLocalHostServer : MonoBehaviour
{
    private LocalHostClientTCP localHostClient;
    public GameObject lobbyPrefab;

    public InputField IPField;
    public InputField PortField;

    private void Start()
    {
        localHostClient = GameObject.FindObjectOfType<LocalHostClientTCP>();
    }

    public void TryConnectingClient()
    {
        IPAddress _serverIP = IPAddress.Parse(IPField.text);
        int _port;
        int.TryParse(PortField.text, out _port);

        if (localHostClient.ConnectToServer(_serverIP, _port))
        {
            Destroy(this.gameObject);
            Instantiate(lobbyPrefab);
        }
    }
}
