using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Text;

public class LocalHostServer : MonoBehaviour
{
	public float timeOutTime = 5f;
	float secondCounter = 0;

	public MyClient owner;
	private int port;

	private List<MyClient> connectedClients = new List<MyClient>();
	private TcpListener _listener;

	private int newPlayerID = 1;

	Room activeRoom;
	public LobbyRoom lobbyRoom;
	public GameRoom gameRoom;

	UdpClient client = new UdpClient();

	private static LocalHostServer _instance;

	public static LocalHostServer Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<LocalHostServer>();
			}

			return _instance;
		}
	}


	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

    private void Start()
    {
		client = new UdpClient(35450);

		try
		{
			client.BeginReceive(new AsyncCallback(recv), null);
		}
		catch (Exception e)
		{
			Debug.Log(e.Message);
		}
	}

    public void Initialize(int _port)
	{
		bool finishedInitialization = false;
		int i = 0;


		while (finishedInitialization == false && i < 20)
		{
			try
			{
				_listener = new TcpListener(IPAddress.Any, _port + i);
				_listener.Start();
				port = _port + i;
				finishedInitialization = true;
				Debug.Log("Server started on port: " + (_port + i));
			}
			catch (Exception e)
			{
				e.ToString();
				i++;
			}
		}
		lobbyRoom = new LobbyRoom();
		lobbyRoom.Initialize(this);
		gameRoom = new GameRoom();
		gameRoom.Initialize(this);

		lobbyRoom.server = this;
		activeRoom = lobbyRoom;
	}

	private void Update()
	{
		ProcessNewClients();
		ProcessExistingClients();
		activeRoom.UpdateRoom();
		SendServerHeartbeats();
	}

	private void ProcessNewClients()
	{
		while (_listener.Pending())
		{
			try
			{
				MyClient newClient = new MyClient(_listener.AcceptTcpClient(), timeOutTime, MyClient.colors.blue, newPlayerID, "");
				newPlayerID += 1;
				lobbyRoom.AddMember(newClient);
				connectedClients.Add(newClient);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}

	private void ProcessExistingClients()
	{
		for (int i = 0; i < connectedClients.Count; i++)
		{
			if (connectedClients[i].tcpClient.Available == 0) continue;

			HandleIncomingMessage(connectedClients[i]);
		}
	}

	private void HandleIncomingMessage(MyClient client)
	{
		try
		{
			byte[] inBytes = NetworkUtils.Read(client.tcpClient.GetStream());
			TCPPacket inPacket = new TCPPacket(inBytes);

			var tempOBJ = inPacket.ReadObject();

			activeRoom.handleTCPNetworkMessageFromUser(tempOBJ, client);
		}
		catch (Exception e)
		{
			Console.WriteLine("Error in Handling Incoming Data: " + e.Message);
		}
	}

	void recv(IAsyncResult res)
	{
		IPEndPoint RemoteIP = new IPEndPoint(IPAddress.Any, 60240);
		byte[] received = client.EndReceive(res, ref RemoteIP);

		UDPPacket packet = new UDPPacket(received);
		var TempOBJ = packet.ReadObject();


		Debug.Log("receiving data on server");
		foreach(MyClient connectedClient in connectedClients)
        {
			if(((IPEndPoint)connectedClient.tcpClient.Client.RemoteEndPoint).Address == RemoteIP.Address)
            {
				activeRoom.handleUDPNetworkMessageFromUser(TempOBJ, connectedClient);
				break;
            }
        }

		client.BeginReceive(new AsyncCallback(recv), null);
	}

	void SendServerHeartbeats()
    {
		secondCounter += Time.deltaTime;
		if(secondCounter > 2)
        {
			TCPPacket outPacket = new TCPPacket();
			HeartBeat request = new HeartBeat();
			outPacket.Write(request);
			SendTCPMessageToAllUsers(outPacket);

			secondCounter = 0;
		}
	}

	public void SetOwner(MyClient client)
    {
		owner = client;
    }

	public int GetServerPort()
	{
		return port;
	}

	public void RemovePlayer(MyClient clientToRemove)
	{	
		Console.WriteLine("removing client from: " + this.GetType());
		clientToRemove.tcpClient.Close();
		connectedClients.Remove(clientToRemove);		
	}

	public void MovePlayersToDifferentRoom(Room originalRoom, Room newRoom)
    {
		foreach(MyClient client in originalRoom.GetMembers())
		{ 
			newRoom.AddMember(client);
        }
		SetActiveRoom(newRoom);
		originalRoom.ClearMembers();
	}

	void SetActiveRoom(Room newActiveRoom)
    {
		activeRoom = newActiveRoom;
    }

	void SendTCPMessageToAllUsers(TCPPacket outPacket)
	{
		foreach (MyClient client in activeRoom.GetMembers())
		{
			try
			{
				NetworkUtils.Write(client.tcpClient.GetStream(), outPacket.GetBytes());
			}
			catch (Exception e)
			{
				Console.WriteLine("Error sending message to target users: " + e.Message);
			}
		}
	}
}
