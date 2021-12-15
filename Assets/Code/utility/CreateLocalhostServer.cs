using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class CreateLocalhostServer : MonoBehaviour
{
    [SerializeField] private int basePort = 55555;

    private LocalHostClientTCP localHostClient;

    public LocalHostServer localHostServerPrefab;
    public GameObject lobbyPrefab;

    public FadingText errorText;

    MySceneManager sceneManager;


    private void Start()
    {
        localHostClient = GameObject.FindObjectOfType<LocalHostClientTCP>();
        sceneManager = GameObject.FindObjectOfType<MySceneManager>();
    }

    public void CreateLocalHostServer()
    {
        try
        {
            LocalHostServer server = Instantiate(localHostServerPrefab);
            server.Initialize(basePort);

            IPAddress _serverIP = Extensions.GetLocalIPAddress();

            ConnectionFeedback connectionFeedback = localHostClient.ConnectToServer(_serverIP, server.GetServerTCPPort());
            if (connectionFeedback.connected)
            {
                sceneManager.LoadScene("LobbyScene");
            }
            else
            {
                Destroy(server.gameObject);
                throw connectionFeedback.error;
            }
        }    
        catch(Exception e)
        {
            errorText.StartFade("Error creating server: " + e.Message);
        }
    }
}
