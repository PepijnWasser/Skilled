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


	//sets the udpClient of the room
	public virtual void Initialize(LocalHostServer _server)
	{
		server = _server;
	}

	//method for adding a member to the room
	public virtual void AddMember(MyClient clientToAdd)
	{
		Debug.Log("moved " + clientToAdd.playerName + " to: " + this.GetType());
		clientsInRoom.Add(clientToAdd);
	}

	//method for removing a member of the room
	public virtual void RemoveMember(MyClient clientToRemove)
	{
		clientsInRoom.Remove(clientToRemove);
		Debug.Log("removing " + clientToRemove.playerName + " from: " + this.GetType());
	}

	//gets a list of all MyClients in room
	public List<MyClient> GetMembers()
	{
		List<MyClient> memberList = clientsInRoom;
		return memberList;
	}

	//clears all members in room
	public virtual void ClearMembers()
    {
		clientsInRoom.Clear();
    }

	//remove a member from room and server
	protected void removeAndCloseMember(MyClient pMember)
	{
		RemoveMember(pMember);
		server.RemovePlayer(pMember);
	}

	//method which is called from server.update
	public virtual void UpdateRoom()
    {
		CheckHeartbeat();
    }

	//checks the heartbeat of all members in room
	protected virtual void CheckHeartbeat()
	{
		List<MyClient> clientsInRoomToRemove = new List<MyClient>();

		for (int i = 0; i < clientsInRoom.Count; i++)
		{
			clientsInRoom[i].heartbeat -= Time.deltaTime;
			if (clientsInRoom[i].heartbeat <= 0)
			{
				clientsInRoomToRemove.Add(clientsInRoom[i]);
				Debug.Log(clientsInRoom[i].playerName + " is being removed because " + clientsInRoom[i].heartbeat);
			}
		}

		foreach (MyClient client in clientsInRoomToRemove)
		{
			Debug.Log("timeout " + client.playerName);
			removeAndCloseMember(client);
		}
	}

	//handling messages
	abstract public void HandleTCPNetworkMessageFromUser(ISerializable pMessage, MyClient pSender);

	abstract public void HandleUDPNetworkMessageFromUser(USerializable pMessage, MyClient pSender);

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

