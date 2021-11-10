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

    MySceneManager sceneManager;

    private void Start()
    {
        localHostClient = GameObject.FindObjectOfType<LocalHostClientTCP>();
        sceneManager = GameObject.FindObjectOfType<MySceneManager>();
    }

    public void TryConnectingClient()
    {
        IPAddress _serverIP = IPAddress.Parse(IPField.text);
        int _port;
        int.TryParse(PortField.text, out _port);

        if (localHostClient.ConnectToServer(_serverIP, _port))
        {
            //Destroy(this.gameObject);
            //Instantiate(lobbyPrefab);
            sceneManager.LoadScene("LobbyScene");
        }
    }
}
