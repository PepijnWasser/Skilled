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

	private string owner;
	private int port;

	List<MyClient> connectedClients = new List<MyClient>();
	private TcpListener _listener;


    public void Initialize(int _port, string _owner)
    {
		owner = _owner;

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
				Debug.Log("Server started on port: "+ (_port + i));
			}
			catch (Exception e)
			{
				i++;
			}
		}

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
				MyClient newClient = new MyClient(_listener.AcceptTcpClient(), timeOutTime, MyClient.colors.blue);
				connectedClients.Add(newClient);

				//update player count for everyone
				Packet outPacket = new Packet();
				UpdatePlayerCountMessage playerCountMessage = new UpdatePlayerCountMessage(connectedClients.Count);
				outPacket.Write(playerCountMessage);
				SendMessageToAllUsers(outPacket);

				//send server name to new user
				Packet outPacket2 = new Packet();
				UpdateServerNameMessage serverNameMessage = new UpdateServerNameMessage(owner);
				outPacket2.Write(serverNameMessage);
				SendMessageToTargetUsers(outPacket2, newClient);
				
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
			else if (tempOBJ is UpdateColorMessage)
            {
				UpdateColorMessage message = tempOBJ as UpdateColorMessage;
				HandleUpdateColorMessage(client, message);
            }
		}
		catch (Exception e)
		{
			Console.WriteLine("Error in Handling Incoming Data: " + e.Message);
		}
	}

	private void RefreshHeartbeat(MyClient pClient)
	{
		pClient.heartbeat = timeOutTime;
	}


	void HandleUpdateColorMessage(MyClient client, UpdateColorMessage message) 
	{
		client.playerColor = Extensions.Next(client.playerColor);

		Packet outPacket = new Packet();
		UpdateColorMessage updateColorMessage = new UpdateColorMessage(client.playerColor);
		outPacket.Write(updateColorMessage);
		SendMessageToAllUsers(outPacket);
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

	public int GetServerPort()
    {
		return port;
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

public static class Extensions
{

	public static T Next<T>(this T src) where T : struct
	{
		if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

		T[] Arr = (T[])Enum.GetValues(src.GetType());
		int j = Array.IndexOf<T>(Arr, src) + 1;
		return (Arr.Length == j) ? Arr[0] : Arr[j];
	}
}
