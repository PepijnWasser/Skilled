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
	private float secondCounter = 0;

	private UdpClient client = new UdpClient();

	//list of all connected clients
	private List<MyClient> connectedClients = new List<MyClient>();
	private TcpListener listener;

	//id given to a new player upon joining
	private int newPlayerID = 1;

	//rooms the server has, and the room which the server needs to update
	public LobbyRoom lobbyRoom;
	public GameRoom gameRoom;
	public EndRoom endRoom;
	private List<Room> activeRooms = new List<Room>();

	//server info
	public ServerInfo serverInfo = new ServerInfo();

	//list of all users and what room the yare in
	private Dictionary<MyClient, Room> playerRoomDictionary = new Dictionary<MyClient, Room>();

	//dictionarys used to move members between rooms
	private Dictionary<Room, Room> movePlayersFromRoomToRoom = new Dictionary<Room, Room>();
	private Dictionary<MyClient, Room> movePlayerToRoom = new Dictionary<MyClient, Room>();


	private static LocalHostServer _instance;
	public static LocalHostServer Instance
	{
		get
		{
			return _instance;
		}
	}

	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		if(_instance != null && _instance != this)
        {
			Destroy(this.gameObject);
        }
        else
        {
			_instance = this;
        }
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
				i++;
			}
		}

		Debug.Log("server UDP port set to: " + serverInfo.udpPort);
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
				listener = new TcpListener(IPAddress.Any, startPort + i);
				listener.Start();
				finishedInitialization = true;
				serverInfo.tcpPort = startPort + i;
				Debug.Log("Server started on port: " + (startPort + i), this.gameObject);
			}
			catch (Exception e)
			{
				i++;
			}
		}

		lobbyRoom = new LobbyRoom();
		activeRooms.Add(lobbyRoom);
		lobbyRoom.Initialize(this);

		gameRoom = new GameRoom();
		activeRooms.Add(gameRoom);
		gameRoom.Initialize(this);

		endRoom = new EndRoom();
		activeRooms.Add(endRoom);
		endRoom.Initialize(this);
	}

	private void Update()
	{
		ProcessNewClients();
		ProcessExistingClients();
		foreach(Room room in activeRooms)
        {
			room.UpdateRoom();
        }
		

		SendServerHeartbeats();
		MovePlayersToDifferentRoom();
		MovePlayerToDifferentRoom();
	}

	//if there is a tcpclient that wants to join, accept and give him a encapsulating MyClient
	private void ProcessNewClients()
	{
		while (listener.Pending())
		{
			try
			{
				MyClient newClient = new MyClient(listener.AcceptTcpClient(), timeOutTime, MyClient.colors.blue, newPlayerID, "TEMP");
				newPlayerID += 1;

				if(serverInfo.serverOwner == null)
                {
					serverInfo.serverOwner = newClient;
				}

				lobbyRoom.AddMember(newClient);
				if (newClient == serverInfo.serverOwner)
				{
					//send server info to new user
					TCPPacket serverInfoPacket = new TCPPacket();
					UpdateServerInfoMessage serverInfoMessage = new UpdateServerInfoMessage(serverInfo.udpPort, serverInfo.tcpPort, serverInfo.ip, serverInfo.serverOwner.playerName, true);
					serverInfoPacket.Write(serverInfoMessage);
					SendTCPMessageToTargetUser(serverInfoPacket, newClient);
				}
				else
				{
					//send server info to new user
					TCPPacket serverInfoPacket = new TCPPacket();
					UpdateServerInfoMessage serverInfoMessage = new UpdateServerInfoMessage(serverInfo.udpPort, serverInfo.tcpPort, serverInfo.ip, serverInfo.serverOwner.playerName, false);
					serverInfoPacket.Write(serverInfoMessage);
					SendTCPMessageToTargetUser(serverInfoPacket, newClient);
				}
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
			{ 
				HandleIncomingMessage(connectedClients[i]);
            }
			
		}
	}

	//send the incoming tcp message to the activeRoom
	private void HandleIncomingMessage(MyClient client)
	{
		//Debug.Log("received TCP message from: " + client.playerName + " in: " + roomOfPlayer);
		try
		{
			byte[] inBytes = NetworkUtils.Read(client.tcpClient.GetStream());
			TCPPacket inPacket = new TCPPacket(inBytes);

			var tempOBJ = inPacket.ReadObject();

			playerRoomDictionary[client].HandleTCPNetworkMessageFromUser(tempOBJ, client);
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
				playerRoomDictionary[connectedClient].HandleUDPNetworkMessageFromUser(TempOBJ, connectedClient);
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

	public void SetRoomOfPlayer(MyClient client, Room roomOfPlayer)
    {
        if (playerRoomDictionary.ContainsKey(client))
        {
			playerRoomDictionary[client] = roomOfPlayer;
        }
        else
        {
			playerRoomDictionary.Add(client, roomOfPlayer);
        }
    }

	public void AddRoomToMoveDictionary(Room originalRoom, Room newRoom)
    {
		movePlayersFromRoomToRoom.Add(originalRoom, newRoom);
	}

	public void AddPlayerToMoveDictionary(MyClient client, Room newRoom)
    {
		movePlayerToRoom.Add(client, newRoom);
    }

	//move players from room x to room y
	void MovePlayersToDifferentRoom()
    {
		foreach (KeyValuePair<Room, Room> entry in movePlayersFromRoomToRoom)
		{
			foreach (MyClient client in entry.Key.GetMembers())
			{
				if(entry.Value is GameRoom)
                {
					GameRoom gameRoom = entry.Value as GameRoom;
					gameRoom.SetRoomsAmount(entry.Key.GetMembers().Count);
                }
				entry.Value.AddMember(client);
			}
			entry.Key.ClearMembers();
		}

		movePlayersFromRoomToRoom.Clear();
	}

	void MovePlayerToDifferentRoom()
    {
		foreach (KeyValuePair<MyClient, Room> entry in movePlayerToRoom)
		{
			foreach(Room room in activeRooms)
            {
				List<MyClient> clientsInRoom = new List<MyClient>(room.GetMembers());
                if (clientsInRoom.Contains(entry.Key))
                {
					room.RemoveMember(entry.Key);
					break;
                }
            }

			entry.Value.AddMember(entry.Key);
		}

		movePlayerToRoom.Clear();
	}


	//send a tcpMessage to all connected clients
	void SendTCPMessageToAllUsers(TCPPacket outPacket)
	{
		foreach (MyClient client in connectedClients)
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

	//send TCP message to target user
	void SendTCPMessageToTargetUser(TCPPacket outPacket, MyClient client)
	{
		NetworkUtils.Write(client.tcpClient.GetStream(), outPacket.GetBytes());
	}
}
