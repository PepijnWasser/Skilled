using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;

public class LobbyRoom : Room
{
    public override void AddMember(MyClient newClient)
    {
		base.AddMember(newClient);
		//update player count for everyone
		TCPPacket outPacket = new TCPPacket();
		UpdatePlayerCountMessage playerCountMessage = new UpdatePlayerCountMessage(clientsInRoom.Count);
		outPacket.Write(playerCountMessage);
		SendTCPMessageToAllUsers(outPacket);

		if (clientsInRoom.Count == 1)
		{
			server.SetOwner(newClient);
			Debug.Log("setting server owner");
		}

		TCPPacket updateIPPacket = new TCPPacket();
		string ownerIP = ((IPEndPoint)server.owner.TcpClient.Client.RemoteEndPoint).Address.ToString();
		int ownerPort = server.GetServerPort();
		UpdateServerIPMessage IPmessage = new UpdateServerIPMessage(ownerIP, ownerPort);
		updateIPPacket.Write(IPmessage);
		SendTCPMessageToTargetUser(updateIPPacket, newClient);


		//send server name to new user
		TCPPacket outPacket2 = new TCPPacket();
		UpdateServerNameMessage serverNameMessage = new UpdateServerNameMessage(server.owner.playerName);
		outPacket2.Write(serverNameMessage);
		SendTCPMessageToTargetUser(outPacket2, newClient);

		//send new player to all users
		TCPPacket outpacket3 = new TCPPacket();
		MakeNewPlayerBarMessage makePlayerBarMessage = new MakeNewPlayerBarMessage(newClient.playerID, newClient.playerColor, newClient.playerName, false);
		outpacket3.Write(makePlayerBarMessage);
		SendTCPMessageToAllUsersExcept(outpacket3, newClient);

		TCPPacket outpacket4 = new TCPPacket();
		MakeNewPlayerBarMessage makePlayerBarMessage2 = new MakeNewPlayerBarMessage(newClient.playerID, newClient.playerColor, newClient.playerName, true);
		outpacket4.Write(makePlayerBarMessage2);
		SendTCPMessageToTargetUser(outpacket4, newClient);

		//send all existing users to new client
		foreach (MyClient client in clientsInRoom)
		{
			if (client != newClient)
			{
				Debug.Log("sending existing clients");
				TCPPacket outpacket5 = new TCPPacket();
				MakeNewPlayerBarMessage makePlayerBarMessage3 = new MakeNewPlayerBarMessage(client.playerID, client.playerColor, client.playerName, false);
				outpacket5.Write(makePlayerBarMessage3);
				SendTCPMessageToTargetUser(outpacket5, newClient);
			}
		}

		if(newClient == server.owner)
        {
			TCPPacket outpacket6 = new TCPPacket();
			ServerOwnerMessage serverOwnerMessage = new ServerOwnerMessage(true);
			outpacket6.Write(serverOwnerMessage);
			SendTCPMessageToTargetUser(outpacket6, newClient);
        }
        else
        {
			TCPPacket outpacket6 = new TCPPacket();
			ServerOwnerMessage serverOwnerMessage = new ServerOwnerMessage(false);
			outpacket6.Write(serverOwnerMessage);
			SendTCPMessageToTargetUser(outpacket6, newClient);
		}

	}

    public override void RemoveMember(MyClient clientToRemove)
    {
        base.RemoveMember(clientToRemove);
		TCPPacket outPacket = new TCPPacket();
		RemovePlayerBarMessage removePlayerBarMessage = new RemovePlayerBarMessage(clientToRemove.playerID);
		outPacket.Write(removePlayerBarMessage);
		SendTCPMessageToAllUsers(outPacket);


		TCPPacket outPacket2 = new TCPPacket();
		UpdatePlayerCountMessage updatePlayerCountMessage = new UpdatePlayerCountMessage(clientsInRoom.Count);
		outPacket2.Write(updatePlayerCountMessage);
		SendTCPMessageToAllUsers(outPacket2);
	}

	public override void handleUDPNetworkMessageFromUser(USerializable pMessage, MyClient pSender)
	{

	}
	public override void handleTCPNetworkMessageFromUser(ISerializable tempOBJ, MyClient client)
	{
		if (tempOBJ is HeartBeat)
		{
			RefreshHeartbeat(client);
		}
		else if (tempOBJ is UpdateColorRequest)
		{
			UpdateColorRequest message = tempOBJ as UpdateColorRequest;
			HandleUpdateColorMessage(client, message);
		}
		else if (tempOBJ is LeaveServermessage)
		{
			LeaveServermessage message = tempOBJ as LeaveServermessage;
			HandleLeaveServerMessage(client, message);
		}
		else if (tempOBJ is UpdatePlayerNameRequest)
		{
			UpdatePlayerNameRequest message = tempOBJ as UpdatePlayerNameRequest;
			HandleUpdateNameRequest(client, message);
		}
		else if (tempOBJ is ChatRequest)
		{
			ChatRequest message = tempOBJ as ChatRequest;
			HandleChatMessage(client, message);
		}
		else if (tempOBJ is HelpRequest)
		{
			HandleHelpRequest(client);
		}
		else if(tempOBJ is StartRoomRequest)
        {
			HandleStartRoomRequest();
        }
	}

    protected override void CheckHeartbeat()
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
			SendPlayerDisconnectMessages(client.playerName);
			removeAndCloseMember(client);
		}
	}

    private void RefreshHeartbeat(MyClient pClient)
	{
		pClient.heartbeat = server.timeOutTime;
	}

	void HandleUpdateNameRequest(MyClient client, UpdatePlayerNameRequest message)
	{
		string requestedName = message.playerName;
		if(requestedName != client.playerName)
        {
			string newName = requestedName;
			int i = 0;
			bool needToCheck = true;
			while (needToCheck == true)
			{
				if (i == 0)
				{
					if (CheckNameAvailible(requestedName))
					{
						needToCheck = false;
						newName = requestedName;
					}
					else
					{
						i += 1;
					}
				}
				else
				{
					if (CheckNameAvailible(requestedName + "(" + i + ")"))
					{
						needToCheck = false;
						newName = requestedName + "(" + i + ")";
					}
					else
					{
						i += 1;
					}
				}
			}

			if(client.playerName != "")
            {
				SendNameChangeChatMessages(client.playerName, newName);
            }
            else
            {
				SendPlayerJoinedMessages(newName);
            }

			UpdatePlayerName(client, newName);

			//if the request was from the owner change server name
			if (client == server.owner)
			{
				UpdateServerName();
			}
		}		
	}

	void HandleHelpRequest(MyClient client)
    {
		string messageToSend =	"/help\n"+
								"/setname";

		DateTime time = DateTime.Now;
		TCPPacket outPacket = new TCPPacket();
		HelpRespons helpRespons = new HelpRespons(messageToSend,"Server", time.Hour, time.Minute, time.Second);
		outPacket.Write(helpRespons);
		SendTCPMessageToTargetUser(outPacket, client);
	}

	void HandleUpdateColorMessage(MyClient client, UpdateColorRequest message)
	{
		if (message.sideToChangeTo > 0)
		{
			client.playerColor = Extensions.Next(client.playerColor);
		}
		else if (message.sideToChangeTo < 0)
		{
			client.playerColor = Extensions.Previous(client.playerColor);
		}


		TCPPacket outPacket = new TCPPacket();
		UpdateColorRespons updateColorRespons = new UpdateColorRespons(client.playerColor, client.playerID);
		outPacket.Write(updateColorRespons);
		SendTCPMessageToAllUsers(outPacket);
	}

	void HandleLeaveServerMessage(MyClient client, LeaveServermessage message)
	{
		SendPlayerLeftMessages(client.playerName);
		removeAndCloseMember(client);
	}

	void HandleChatMessage(MyClient client, ChatRequest message)
    {
		DateTime time = DateTime.Now;

		TCPPacket outPacket = new TCPPacket();
		ChatRespons chatMessage = new ChatRespons(message.chatMessage, client.playerName, time.Hour, time.Minute, time.Second);
		outPacket.Write(chatMessage);
		SendTCPMessageToAllUsers(outPacket);
    }

	void HandleStartRoomRequest()
    {
		TCPPacket outPacket = new TCPPacket();
		JoinRoomMessage startRoomMessage = new JoinRoomMessage(JoinRoomMessage.rooms.game);
		outPacket.Write(startRoomMessage);
		SendTCPMessageToAllUsers(outPacket);
		server.MovePlayersToDifferentRoom(this, server.gameRoom);
    }

	void SendPlayerLeftMessages(string playerWhoLeft)
    {
		DateTime time = DateTime.Now;
		string messageToSend = playerWhoLeft + " left the room";

		TCPPacket outPacket = new TCPPacket();
		ChatRespons chatMessage = new ChatRespons(messageToSend, "Server", time.Hour, time.Minute, time.Second);
		outPacket.Write(chatMessage);
		SendTCPMessageToAllUsers(outPacket);
	}

	void SendPlayerJoinedMessages(string playerWhoJoined)
    {
		DateTime time = DateTime.Now;
		string messageToSend = playerWhoJoined + " joined the room";

		TCPPacket outPacket = new TCPPacket();
		ChatRespons chatMessage = new ChatRespons(messageToSend, "Server", time.Hour, time.Minute, time.Second);
		outPacket.Write(chatMessage);
		SendTCPMessageToAllUsers(outPacket);
	}

	void SendPlayerDisconnectMessages(string playerWhoDisconnected)
    {
		DateTime time = DateTime.Now;
		string messageToSend = playerWhoDisconnected + " timed out";

		TCPPacket outPacket = new TCPPacket();
		ChatRespons chatMessage = new ChatRespons(messageToSend, "Server", time.Hour, time.Minute, time.Second);
		outPacket.Write(chatMessage);
		SendTCPMessageToAllUsers(outPacket);
	}
		
	void SendNameChangeChatMessages(string oldName, string newName)
    {
		DateTime time = DateTime.Now;
		string messageToSend = oldName + " is now known as " + newName;

		TCPPacket outPacket = new TCPPacket();
		ChatRespons chatMessage = new ChatRespons(messageToSend, "Server", time.Hour, time.Minute, time.Second);
		outPacket.Write(chatMessage);
		SendTCPMessageToAllUsers(outPacket);
	}

	void UpdatePlayerName(MyClient client, string newName)
    {
		client.playerName = newName;

		TCPPacket outPacket = new TCPPacket();
		UpdatePlayerNameRespons updateNameRespons = new UpdatePlayerNameRespons(client.playerName, client.playerID);
		outPacket.Write(updateNameRespons);
		SendTCPMessageToAllUsers(outPacket);
	}

	void UpdateServerName()
    {
		TCPPacket outPacket = new TCPPacket();
		UpdateServerNameMessage updateserverNameMessage = new UpdateServerNameMessage(server.owner.playerName);
		outPacket.Write(updateserverNameMessage);
		SendTCPMessageToAllUsers(outPacket);
	}

	bool CheckNameAvailible(string name)
	{
		foreach (MyClient c in clientsInRoom)
		{
			if (c.playerName == name)
			{
				return false;
			}
		}
		return true;
	}
}
