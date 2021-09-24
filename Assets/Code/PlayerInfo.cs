using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class PlayerInfo : MonoBehaviour
{
	public int playerID;
	public string playerName;
	public int udpPort;
	public IPAddress ip;

	private static PlayerInfo _instance;

	public static PlayerInfo Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<PlayerInfo>();
			}

			return _instance;
		}
	}

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

    private void Start()
    {
		ip = Extensions.GetLocalIPAddress();
    }
}
