using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System;

public class JoinLocalHostServer : MonoBehaviour
{
    private LocalHostClientTCP localHostClient;
    public GameObject lobbyPrefab;

    public InputField IPField;
    public InputField PortField;

    public FadingText errorText;

    MySceneManager sceneManager;

    private void Start()
    {
        localHostClient = GameObject.FindObjectOfType<LocalHostClientTCP>();
        sceneManager = GameObject.FindObjectOfType<MySceneManager>();
    }

    public void TryConnectingClient()
    {
        try
        {
            IPAddress _serverIP = IPAddress.Parse(IPField.text);
            int _port;
            int.TryParse(PortField.text, out _port);;

            ConnectionFeedback connectionFeedback = localHostClient.ConnectToServer(_serverIP, _port);
            if (connectionFeedback.connected)
            {
                sceneManager.LoadScene("LobbyScene");
            }
            else
            {
                throw connectionFeedback.error;
            }
        }
        catch(Exception e)
        {
            errorText.StartFade("Error joining server: " + e.Message);
        }
    }
}
