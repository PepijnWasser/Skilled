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

	public virtual void AddMember(MyClient clientToAdd)
	{
		Debug.Log("accepted new member to: " + this.GetType());
		clientsInRoom.Add(clientToAdd);
	}

	public virtual void RemoveMember(MyClient clientToRemove)
	{
		clientsInRoom.Remove(clientToRemove);
		Debug.Log("removing member from: " + this.GetType());
	}

	public List<MyClient> GetMembers()
	{
		List<MyClient> memberList = new List<MyClient>(clientsInRoom);
		return memberList;
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

	void CheckHeartbeat()
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
			Debug.Log("timeout");
			removeAndCloseMember(client);
		}
	}

	abstract public void handleNetworkMessageFromUser(ISerializable pMessage, MyClient pSender);

	protected void SendMessageToTargetUser(Packet outPacket, MyClient client)
	{
		StreamUtil.Write(client.TcpClient.GetStream(), outPacket.GetBytes());
	}

	protected void SendMessageToAllUsersExcept(Packet outPacket, MyClient clientToAvoid)
	{
		foreach (MyClient client in clientsInRoom)
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

	protected void SendMessageToAllUsers(Packet outPacket)
	{
		foreach (MyClient client in clientsInRoom)
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
}

public static class Extensions
{

	public static T Next<T>(this T src) where T : struct
	{
		if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

		T[] Arr = (T[])Enum.GetValues(src.GetType());
		int j = Array.IndexOf<T>(Arr, src) + 1;
		if (Arr.Length == j)
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
		if (j < 0)
		{
			return Arr[Arr.Length - 1];
		}
		else
		{
			return Arr[j];
		}
	}
}
