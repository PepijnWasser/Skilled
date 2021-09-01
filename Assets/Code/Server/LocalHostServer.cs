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

	List<MyClient> connectedClients = new List<MyClient>();
	int newPlayerID = 0;
	private TcpListener _listener;


    public void Initialize()
    {
		Debug.Log("Server started on port 55555");

		_listener = new TcpListener(IPAddress.Any, 55555);
		_listener.Start();
	}

    private void Update()
    {
		ProcessNewClients();
		ProcessExistingClients();
		CheckHeartbeat(connectedClients);
		//Although technically not required, now that we are no longer blocking, 
		//it is good to cut your CPU some slack
		//Thread.Sleep(100);
	}

    private void ProcessNewClients()
	{
		while (_listener.Pending())
		{
			try
			{
				MyClient newClient = new MyClient(_listener.AcceptTcpClient(), newPlayerID, timeOutTime);
				connectedClients.Add(newClient);
				newPlayerID++;

				Packet outPacket = new Packet();
				UpdatePlayerCountMessage playerCountMessage = new UpdatePlayerCountMessage(connectedClients.Count);
				outPacket.Write(playerCountMessage);
				SendMessageToAllUsers(outPacket);

				Debug.Log("Accepted new client.");
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

			HandleIncomingData(connectedClients[i]);
		}
	}

	private void HandleIncomingData(MyClient client)
	{
		try
		{
			byte[] inBytes = StreamUtil.Read(client.TcpClient.GetStream());
			Packet inPacket = new Packet(inBytes);

			var tempOBJ = inPacket.ReadObject();

			if (tempOBJ is Heartbeat)
			{
				RefreshHeartbeat(client);
			}
		}
		catch (Exception e)
		{
			Console.WriteLine("Error in Handling Incoming Data: " + e.Message);
		}
	}

	/*
	private void HandleChat(ChatMessage message, MyClient client)
	{
		try
		{
			Console.WriteLine("Received a chatmessage");
			Packet outPacket = new Packet();
			ChatMessage outMessage = new ChatMessage(message.text, client.playerID);
			outPacket.Write(outMessage);

			SendMessageToAllUsers(outPacket);
		}
		catch (Exception e)
		{
			Console.WriteLine("Error: " + e.Message);
		}
	}

	private void HandleMoving(AgentMovingMessage message, MyClient client)
	{
		try
		{
			Console.WriteLine("Received new position");
			client.avatarModel.posX = message.posX;
			client.avatarModel.posY = message.posY;
			client.avatarModel.posZ = message.posZ;
			client.avatarModel.posW = message.posW;

			Packet outPacket = new Packet();
			MoveAgentMessage outMessage = new MoveAgentMessage(client.avatarModel, client.playerID);

			outPacket.Write(outMessage);

			SendMessageToAllUsers(outPacket);
		}
		catch (Exception e)
		{
			Console.WriteLine("Error: " + e.Message);
		}
	}
	*/

	private void RefreshHeartbeat(MyClient pClient)
	{
		pClient.heartbeat = timeOutTime;
	}

	private void SendMessageToAllUsers(Packet outPacket)
	{
		foreach (MyClient client in connectedClients)
		{
			try
			{
				StreamUtil.Write(client.TcpClient.GetStream(), outPacket.GetBytes());
			}
			catch (Exception e)
			{
				Console.WriteLine("Error sending message to target users: " + e.Message);
			}
		}
	}

	void CheckHeartbeat(List<MyClient> connectedClients)
	{
		List<MyClient> connectedClientsToRemove = new List<MyClient>();

		for (int i = 0; i < connectedClients.Count; i++)
		{
			connectedClients[i].heartbeat -= Time.deltaTime;
			Debug.Log(connectedClients[i].heartbeat);
			if (connectedClients[i].heartbeat <= 0)
			{
				connectedClientsToRemove.Add(connectedClients[i]);
			}
		}

		foreach (MyClient client in connectedClientsToRemove)
		{
			//to do
			//disconnect message to disconnected user



			Console.WriteLine("removing client");
			client.TcpClient.Close();
			connectedClients.Remove(client);

			Packet outPacket = new Packet();
			UpdatePlayerCountMessage playerCountMessage = new UpdatePlayerCountMessage(connectedClients.Count);
			outPacket.Write(playerCountMessage);
			SendMessageToAllUsers(outPacket);
		}
	}


	private void SendMessageToTargetUsers(Packet outPacket, MyClient client)
	{
		StreamUtil.Write(client.TcpClient.GetStream(), outPacket.GetBytes());
	}

	private void SendMessageToAllUsersExcept(Packet outPacket, MyClient clientToAvoid)
	{
		foreach (MyClient client in connectedClients)
		{
			if (client != clientToAvoid)
			{
				try
				{
					StreamUtil.Write(client.TcpClient.GetStream(), outPacket.GetBytes());
				}
				catch (Exception e)
				{
					Console.WriteLine("Error sending message to all users: " + e.Message);
				}
			}
		}
	}
}
