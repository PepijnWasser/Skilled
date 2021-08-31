using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public GameObject LocalHostServerPrefab;

    public void CreateLocalHostServer()
    {
        Instantiate(LocalHostServerPrefab);
    }
}
