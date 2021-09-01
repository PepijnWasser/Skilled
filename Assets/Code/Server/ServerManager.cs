using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class ServerManager : MonoBehaviour
{
    [SerializeField] private string _server = "localhost";
    [SerializeField] private int basePort = 55555;

    public LocalHostServer LocalHostServerPrefab;

    public LocalHostClient LocalHostClient;

    public GameObject ServerScreen;
    public GameObject CreateAndJoinScreen;
    public GameObject LobbyListScreen;

    public PlayerInfo playerInfo;

    public void CreateLocalHostServer()
    {
        LocalHostServer server = Instantiate(LocalHostServerPrefab);
        server.Initialize(basePort, playerInfo.playerName);

        if (LocalHostClient.ConnectToServer(_server, server.GetServerPort()))
        {
            CreateAndJoinScreen.SetActive(false);
            ServerScreen.SetActive(true);
        }
        else
        {
            CreateAndJoinScreen.SetActive(true);
            ServerScreen.SetActive(false);
            Destroy(server.gameObject);
        }
    }

    public void TryConnectingClient(int port)
    {
        if (LocalHostClient.ConnectToServer(_server, port))
        {
            CreateAndJoinScreen.SetActive(false);
            ServerScreen.SetActive(true);
            LobbyListScreen.SetActive(false);
        }
        else
        {
            CreateAndJoinScreen.SetActive(true);
            ServerScreen.SetActive(false);
        }
    }
}
