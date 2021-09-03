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

	private List<MyClient> connectedClients = new List<MyClient>();
	private TcpListener _listener;

	private int newPlayerID = 0;


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
				string newPlayerName = "Player " + newPlayerID;
				MyClient newClient = new MyClient(_listener.AcceptTcpClient(), timeOutTime, MyClient.colors.blue, newPlayerID, newPlayerName);
				newPlayerID += 1;

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
				SendMessageToTargetUser(outPacket2, newClient);

				//send new player to all users except the new one
				Packet outpacket3 = new Packet();
				MakeNewPlayerBarMessage playerBarMessage = new MakeNewPlayerBarMessage(newClient.playerID, newClient.playerColor, newClient.playerName, false);
				outpacket3.Write(playerBarMessage);
				SendMessageToAllUsersExcept(outpacket3, newClient);
				
				//send new player to all users except the new one
				Packet outpacket4 = new Packet();
				MakeNewPlayerBarMessage playerBarMessage2 = new MakeNewPlayerBarMessage(newClient.playerID, newClient.playerColor, newClient.playerName, true);
				outpacket4.Write(playerBarMessage2);
				SendMessageToTargetUser(outpacket4, newClient);
				
				//send all existing users to new client
				foreach (MyClient client in connectedClients)
                {
					if(client != newClient)
                    {
						Debug.Log("sending existing client");
						Packet outpacket5 = new Packet();
						MakeNewPlayerBarMessage playerBarMessage3 = new MakeNewPlayerBarMessage(client.playerID, client.playerColor, client.playerName, false);
						outpacket5.Write(playerBarMessage3);
						SendMessageToTargetUser(outpacket5, newClient);
					}
                }

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
			else if (tempOBJ is RequestColorChangeMessage)
            {
				RequestColorChangeMessage message = tempOBJ as RequestColorChangeMessage;
				HandleUpdateColorMessage(client, message);
            }
			else if(tempOBJ is LeaveServermessage)
            {
				LeaveServermessage message = tempOBJ as LeaveServermessage;
				HandleLeaveServerMessage(client, message);
            }
			else if(tempOBJ is UpdatePlayerNameRequest)
            {
				UpdatePlayerNameRequest message = tempOBJ as UpdatePlayerNameRequest;
				HandleUpdateNameRequest(client, message);
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

	void HandleUpdateNameRequest(MyClient client, UpdatePlayerNameRequest message)
    {
		client.playerName = message.playerName;

		Packet outPacket = new Packet();
		UpdatePlayerNameRespons updateNameRespons = new UpdatePlayerNameRespons(client.playerName, client.playerID);
		outPacket.Write(updateNameRespons);
		SendMessageToAllUsers(outPacket);
	}

	void HandleUpdateColorMessage(MyClient client, RequestColorChangeMessage message) 
	{
		if(message.sideToChangeTo > 0)
        {
			client.playerColor = Extensions.Next(client.playerColor);
		}
		else if(message.sideToChangeTo < 0)
        {
			client.playerColor = Extensions.Previous(client.playerColor);
		}


		Packet outPacket = new Packet();
		UpdateColorMessage updateColorMessage = new UpdateColorMessage(client.playerColor, client.playerID);
		outPacket.Write(updateColorMessage);
		SendMessageToAllUsers(outPacket);
	}

	void HandleLeaveServerMessage(MyClient client, LeaveServermessage message)
    {
		RemovePlayer(client);
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
			RemovePlayer(client);
		}
	}

	public int GetServerPort()
    {
		return port;
    }

	private void RemovePlayer(MyClient clientToRemove)
    {
		Console.WriteLine("removing client");
		clientToRemove.TcpClient.Close();
		connectedClients.Remove(clientToRemove);

		Packet outPacket = new Packet();
		RemovePlayerBarMessage removePlayerBarMessage = new RemovePlayerBarMessage(clientToRemove.playerID);
		outPacket.Write(removePlayerBarMessage);
		SendMessageToAllUsers(outPacket);


		Packet outPacket2 = new Packet();
		UpdatePlayerCountMessage playerCountMessage = new UpdatePlayerCountMessage(connectedClients.Count);
		outPacket2.Write(playerCountMessage);
		SendMessageToAllUsers(outPacket2);
	}

	private void SendMessageToTargetUser(Packet outPacket, MyClient client)
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
		if(Arr.Length == j)
        {
			return Arr[0];
        }
        else
        {
			return Arr[j];
        }
	}

	public static T Previous<T>(this T src) where T : struct
	{
		//if its not an enum throw an error
		if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

		//get an array of all keys in enum
		T[] Arr = (T[])Enum.GetValues(src.GetType());
		int j = Array.IndexOf<T>(Arr, src) - 1;
		if(j < 0)
        {
			return Arr[Arr.Length - 1];
        }
        else
        {
			return Arr[j];
        }
	}
}
