using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;

public class LocalHostServer : MonoBehaviour
{
	public float timeOutTime = 5f;

	public MyClient owner;
	private int port;

	private List<MyClient> connectedClients = new List<MyClient>();
	private TcpListener _listener;

	private int newPlayerID = 0;

	Room activeRoom;
	LobbyRoom lobbyRoom;


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
				i++;
			}
		}
		lobbyRoom = new LobbyRoom();
		lobbyRoom.server = this;
		activeRoom = lobbyRoom;
	}

	private void Update()
	{
		ProcessNewClients();
		ProcessExistingClients();
		activeRoom.UpdateRoom();
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
			if (connectedClients[i].TcpClient.Available == 0) continue;

			HandleIncomingMessage(connectedClients[i]);
		}
	}

	private void HandleIncomingMessage(MyClient client)
	{
		try
		{
			byte[] inBytes = StreamUtil.Read(client.TcpClient.GetStream());
			Packet inPacket = new Packet(inBytes);

			var tempOBJ = inPacket.ReadObject();
			Debug.Log("data icoming: " + tempOBJ.GetType());

			activeRoom.handleNetworkMessageFromUser(tempOBJ, client);
		}
		catch (Exception e)
		{
			Console.WriteLine("Error in Handling Incoming Data: " + e.Message);
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
		clientToRemove.TcpClient.Close();
		connectedClients.Remove(clientToRemove);
	}
}
