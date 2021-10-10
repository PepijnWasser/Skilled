using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour
{
    public GameObject playerBarPrefab;
    public GameObject verticalLayoutGroupPlayers;
    public GameObject startRoomButton;
    public Text serverIP;
    public Text serverPort;

    //dictionary to keep track of all players
    Dictionary<int, PlayerDataMenu> lobbyPlayers = new Dictionary<int, PlayerDataMenu>();

    //adds a new player bar to the lobby view
    public void AddPlayer(int playerID, string playerColor, string playerName, bool isPlayer)
    {
        PlayerDataMenu newPlayerData = new PlayerDataMenu(playerColor, playerName);
        lobbyPlayers.Add(playerID, newPlayerData);
        
        GameObject newPlayerBar = Instantiate(playerBarPrefab, verticalLayoutGroupPlayers.transform);
        newPlayerData.AddPlayerBar(newPlayerBar);

        UpdatePlayerColor(playerID, playerColor);
        UpdateName(playerID, playerName);
        SetBarOwnership(playerID, isPlayer);

    }

    //removes a player from the view
    public void RemovePlayer(int playerID)
    {
        Destroy(lobbyPlayers[playerID].playerBar.gameObject);
        lobbyPlayers.Remove(playerID);
    }

    //updates color of a player
    public void UpdatePlayerColor(int playerID, string _color)
    {
        System.Drawing.Color c = System.Drawing.Color.FromName(_color);
        Color unityColor = new Color32(c.R, c.G, c.B, c.A);
        lobbyPlayers[playerID].playerColor = unityColor;
        lobbyPlayers[playerID].playerBar.GetComponent<PlayerBarManager>().SwitchColor(unityColor);
    }

    //updates name of a player
    public void UpdateName(int playerID, string name)
    {
        lobbyPlayers[playerID].playerName = name;
        lobbyPlayers[playerID].playerBar.GetComponent<PlayerBarManager>().SetName(name);
    }
    
    //set the bar given by id to isPlayer
    //every player is the owner of 1 bar to change things like color
    public void SetBarOwnership(int playerID, bool isPlayer)
    {
        lobbyPlayers[playerID].playerBar.GetComponent<PlayerBarManager>().SetOwnership(isPlayer);
    }

    //if this player is the server owner display the start button, else hide it
    public void SetServerOwner(bool isOwner)
    {
        startRoomButton.gameObject.SetActive(isOwner);
    }

    //sets the server IP in the lobby view
    public void SetServerIP(string ip)
    {
        serverIP.text = "IP: " + ip;
    }

    //sets the server port in teh lobby view
    public void SetServerPort(int port)
    {
        serverPort.text = "Port: " + port.ToString();
    }
}
