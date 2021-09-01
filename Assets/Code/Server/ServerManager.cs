using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public LocalHostServer LocalHostServerPrefab;

    public LocalHostClient LocalHostClient;

    public GameObject ServerScreen;
    public GameObject CreateAndJoinScreen;

    public void CreateLocalHostServer()
    {
        LocalHostServer server = Instantiate(LocalHostServerPrefab);
        server.Initialize();

        if (LocalHostClient.ConnectToServer())
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

    public void TryConnectingClient()
    {
        if (LocalHostClient.ConnectToServer())
        {
            CreateAndJoinScreen.SetActive(false);
            ServerScreen.SetActive(true);
        }
        else
        {
            CreateAndJoinScreen.SetActive(true);
            ServerScreen.SetActive(false);
        }
    }
}
