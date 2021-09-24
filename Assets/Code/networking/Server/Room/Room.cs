using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;

public abstract class Room
{
	public LocalHostServer server;
	protected List<MyClient> clientsInRoom = new List<MyClient>();

	UdpClient client = new UdpClient();

	public virtual void Initialize(LocalHostServer _server)
	{
		server = _server;
		int i = 0;
		bool finishedInitialization = false;

		while (finishedInitialization == false && i < 20)
		{
			try
			{
				client = new UdpClient(45645 + i);
				finishedInitialization = true;
			}
			catch (Exception e)
			{
				e.ToString();
				i++;
			}
		}
	}

	public virtual void AddMember(MyClient clientToAdd)
	{
		Debug.Log("accepted new member to: " + this.GetType());
		clientsInRoom.Add(clientToAdd);
	}

	public virtual void RemoveMember(MyClient clientToRemove)
	{
		clientsInRoom.Remove(clientToRemove);
		Debug.Log("removing " + clientToRemove.playerName + " from: " + this.GetType());
	}

	public List<MyClient> GetMembers()
	{
		List<MyClient> memberList = clientsInRoom;
		return memberList;
	}

	public virtual void ClearMembers()
    {
		clientsInRoom.Clear();
    }

	protected void removeAndCloseMember(MyClient pMember)
	{
		RemoveMember(pMember);
		server.RemovePlayer(pMember);
	}

	public virtual void UpdateRoom()
    {
		CheckHeartbeat();
    }

	protected virtual void CheckHeartbeat()
	{
		List<MyClient> clientsInRoomToRemove = new List<MyClient>();

		for (int i = 0; i < clientsInRoom.Count; i++)
		{
			clientsInRoom[i].heartbeat -= Time.deltaTime;
			if (clientsInRoom[i].heartbeat <= 0)
			{
				clientsInRoomToRemove.Add(clientsInRoom[i]);
			}
		}

		foreach (MyClient client in clientsInRoomToRemove)
		{
			Debug.Log("timeout" + client.playerName);
			removeAndCloseMember(client);
		}
	}
	//handling messages
	abstract public void handleTCPNetworkMessageFromUser(ISerializable pMessage, MyClient pSender);

	abstract public void handleUDPNetworkMessageFromUser(USerializable pMessage, MyClient pSender);

	//sending messages

	protected void SendTCPMessageToTargetUser(TCPPacket outPacket, MyClient client)
	{
		NetworkUtils.Write(client.tcpClient.GetStream(), outPacket.GetBytes());
	}

	protected void SendTCPMessageToAllUsersExcept(TCPPacket outPacket, MyClient clientToAvoid)
	{
		foreach (MyClient client in clientsInRoom)
		{
			if (client != clientToAvoid)
			{
				try
				{
					NetworkUtils.Write(client.tcpClient.GetStream(), outPacket.GetBytes());
				}
				catch (Exception e)
				{
					Console.WriteLine("Error sending message to all users: " + e.Message);
				}
			}
		}
	}

	protected void SendTCPMessageToAllUsers(TCPPacket outPacket)
	{
		foreach (MyClient client in clientsInRoom)
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

	protected void SendUDPMessageToTargetUser(UDPPacket outPacket, MyClient myClient)
	{
		IPEndPoint RemoteIP = new IPEndPoint(myClient.endPoint.Address, myClient.endPoint.Port);

		byte[] sendBytes = outPacket.GetBytes();

		client.Send(sendBytes, sendBytes.Length, RemoteIP);
	}

	protected void SendUDPMessageToAllUsersExcept(UDPPacket outPacket, MyClient clientToAvoid)
	{
		foreach (MyClient myClient in clientsInRoom)
		{
			if (myClient != clientToAvoid)
			{
				try
				{
					IPEndPoint RemoteIP = new IPEndPoint(myClient.endPoint.Address, myClient.endPoint.Port);
					Debug.Log(myClient.playerName + "     " + RemoteIP.Address + "    " + RemoteIP.Port);

					byte[] sendBytes = outPacket.GetBytes();

					client.Send(sendBytes, sendBytes.Length, RemoteIP);
				}
				catch (Exception e)
				{
					Console.WriteLine("Error sending message to all users: " + e.Message);
				}
			}
		}
	}

	protected void SendUDPMessageToAllUsers(UDPPacket outPacket)
	{
		foreach (MyClient myClient in clientsInRoom)
		{
			try
			{
				IPEndPoint RemoteIP = new IPEndPoint(myClient.endPoint.Address, myClient.endPoint.Port);

				byte[] sendBytes = outPacket.GetBytes();

				client.Send(sendBytes, sendBytes.Length, RemoteIP);
			}
			catch (Exception e)
			{
				Console.WriteLine("Error sending message to all users: " + e.Message);
			}

		}
	}
}

