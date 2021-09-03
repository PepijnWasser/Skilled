using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;

public class LobbyRoom : Room
{
	public override void handleNetworkMessageFromUser(ISerializable tempOBJ, MyClient client)
	{
		if (tempOBJ is Heartbeat)
		{
			RefreshHeartbeat(client);
		}
		else if (tempOBJ is RequestColorChangeMessage)
		{
			RequestColorChangeMessage message = tempOBJ as RequestColorChangeMessage;
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
	}

    public override void AddMember(MyClient newClient)
    {
		base.AddMember(newClient);
		//update player count for everyone
		Packet outPacket = new Packet();
		UpdatePlayerCountMessage playerCountMessage = new UpdatePlayerCountMessage(clientsInRoom.Count);
		outPacket.Write(playerCountMessage);
		SendMessageToAllUsers(outPacket);

		if (clientsInRoom.Count == 1)
		{
			server.SetOwner(newClient);
			Debug.Log("setting server owner");
		}
		
		//send server name to new user
		Packet outPacket2 = new Packet();
		UpdateServerNameMessage serverNameMessage = new UpdateServerNameMessage(server.owner.playerName);
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
		foreach (MyClient client in clientsInRoom)
		{
			if (client != newClient)
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

    public override void RemoveMember(MyClient clientToRemove)
    {
        base.RemoveMember(clientToRemove);
		Packet outPacket = new Packet();
		RemovePlayerBarMessage removePlayerBarMessage = new RemovePlayerBarMessage(clientToRemove.playerID);
		outPacket.Write(removePlayerBarMessage);
		SendMessageToAllUsers(outPacket);


		Packet outPacket2 = new Packet();
		UpdatePlayerCountMessage playerCountMessage = new UpdatePlayerCountMessage(clientsInRoom.Count);
		outPacket2.Write(playerCountMessage);
		SendMessageToAllUsers(outPacket2);
	}

    private void RefreshHeartbeat(MyClient pClient)
	{
		pClient.heartbeat = server.timeOutTime;
	}

	void HandleUpdateNameRequest(MyClient client, UpdatePlayerNameRequest message)
	{
		string requestedName = message.playerName;
		string availibleName = requestedName;
		int i = 0;
		bool needToCheck = true;
		while (needToCheck == true)
		{
			if (i == 0)
			{
				if (CheckNameAvailible(requestedName))
				{
					needToCheck = false;
					availibleName = requestedName;
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
					availibleName = requestedName + "(" + i + ")";
				}
				else
				{
					i += 1;
				}
			}
		}

		client.playerName = availibleName;

		Packet outPacket = new Packet();
		UpdatePlayerNameRespons updateNameRespons = new UpdatePlayerNameRespons(client.playerName, client.playerID);
		outPacket.Write(updateNameRespons);
		SendMessageToAllUsers(outPacket);


		//if the request was from the owner change server name
		if (client == server.owner)
		{
			Packet outPacket2 = new Packet();
			UpdateServerNameMessage serverNameMessage = new UpdateServerNameMessage(server.owner.playerName);
			outPacket2.Write(serverNameMessage);
			SendMessageToAllUsers(outPacket2);
		}

	}

	void HandleUpdateColorMessage(MyClient client, RequestColorChangeMessage message)
	{
		if (message.sideToChangeTo > 0)
		{
			client.playerColor = Extensions.Next(client.playerColor);
		}
		else if (message.sideToChangeTo < 0)
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
		removeAndCloseMember(client);
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
