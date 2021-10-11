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
	//if a heartbeat isn't received within the timeOutTime, kick the client
	public float timeOutTime = 5f;
	float secondCounter = 0;

	UdpClient client = new UdpClient();

	//list of all connected clients
	private List<MyClient> connectedClients = new List<MyClient>();
	private TcpListener _listener;

	//id given to a new player upon joining
	private int newPlayerID = 1;

	//rooms the server has, and the room which the server needs to update
	Room activeRoom;
	public LobbyRoom lobbyRoom;
	public GameRoom gameRoom;

	//server info
	public ServerInfo serverInfo = new ServerInfo();

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
		// while port x where x starts at 35450 is already in use, increase x by one and start listening for UDP communication on that port
		int i = 0;
		bool finishedInitialization = false;

		while (finishedInitialization == false && i < 20)
		{
			try
			{
				client = new UdpClient(35450 + i);
				finishedInitialization = true;
				client.BeginReceive(new AsyncCallback(recv), null);
				serverInfo.udpPort = 35450 + i;
				serverInfo.ip = Extensions.GetLocalIPAddress();
			}
			catch (Exception e)
			{
				e.ToString();
				i++;
			}
		}
	}

	//set the server port for tcp communication
    public void Initialize(int startPort)
	{
		bool finishedInitialization = false;
		int i = 0;


		while (finishedInitialization == false && i < 20)
		{
			try
			{
				_listener = new TcpListener(IPAddress.Any, startPort + i);
				_listener.Start();
				finishedInitialization = true;
				serverInfo.tcpPort = startPort + i;
				Debug.Log("Server started on port: " + (startPort + i));
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

	//if there is a tcpclient that wants to join, accept and give him a MyClient
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

	//check tcpCommunication for each client
	private void ProcessExistingClients()
	{
		for (int i = 0; i < connectedClients.Count; i++)
		{
			if (connectedClients[i].tcpClient.Available == 0) continue;

			HandleIncomingMessage(connectedClients[i]);
		}
	}

	//send the incoming tcp message to the activeRoom
	private void HandleIncomingMessage(MyClient client)
	{
		try
		{
			byte[] inBytes = NetworkUtils.Read(client.tcpClient.GetStream());
			TCPPacket inPacket = new TCPPacket(inBytes);

			var tempOBJ = inPacket.ReadObject();

			activeRoom.HandleTCPNetworkMessageFromUser(tempOBJ, client);
		}
		catch (Exception e)
		{
			Console.WriteLine("Error in Handling Incoming Data: " + e.Message);
		}
	}

	//receive function for udpCommunication
	//checks all clients in server if the incoming ip/port is from that user
	void recv(IAsyncResult res)
	{
		IPEndPoint RemoteIP = new IPEndPoint(IPAddress.Any, 60240);
		byte[] received = client.EndReceive(res, ref RemoteIP);


		UDPPacket packet = new UDPPacket(received);
		var TempOBJ = packet.ReadObject();

		foreach(MyClient connectedClient in connectedClients)
        {
			if (connectedClient.endPoint.Address.ToString() == RemoteIP.Address.ToString() && connectedClient.sendPort.ToString() == RemoteIP.Port.ToString())
            {
				activeRoom.HandleUDPNetworkMessageFromUser(TempOBJ, connectedClient);
				Debug.Log("received UDP message from: " + connectedClient.playerName);
				break;
            }
        }

		client.BeginReceive(new AsyncCallback(recv), null);
	}

	//send heartbeat to all clients to let them know the server is still active
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

	//set the server owner
	public void SetOwner(MyClient client)
    {
		serverInfo.serverOwner = client;
    }

	//gets the tcpport the server is listening on
	public int GetServerTCPPort()
	{
		return serverInfo.tcpPort;
	}

	//remove a player from the server
	public void RemovePlayer(MyClient clientToRemove)
	{	
		Console.WriteLine("removing client from: " + this.GetType());
		clientToRemove.tcpClient.Close();
		connectedClients.Remove(clientToRemove);		
	}

	//move players from room x to room y
	public void MovePlayersToDifferentRoom(Room originalRoom, Room newRoom)
    {
		foreach(MyClient client in originalRoom.GetMembers())
		{ 
			newRoom.AddMember(client);
        }
		SetActiveRoom(newRoom);
		originalRoom.ClearMembers();
	}

	//sets the active room
	void SetActiveRoom(Room newActiveRoom)
    {
		activeRoom = newActiveRoom;
    }

	//send a tcpMessage to all connected clients
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
