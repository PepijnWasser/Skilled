using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class ServerConnectionData : MonoBehaviour
{
    public int udpPort;
    public int tcpPort;
    public IPAddress ip;
    public string owner;
    public bool isOwner;

    private static ServerConnectionData _instance;

    public static ServerConnectionData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ServerConnectionData>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

}
