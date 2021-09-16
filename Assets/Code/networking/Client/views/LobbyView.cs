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


    Dictionary<int, PlayerDataMenu> lobbyPlayers = new Dictionary<int, PlayerDataMenu>();


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

    public void RemovePlayer(int playerID)
    {
        Destroy(lobbyPlayers[playerID].playerBar.gameObject);
        lobbyPlayers.Remove(playerID);
    }

    public void UpdatePlayerColor(int playerID, string _color)
    {
        System.Drawing.Color c = System.Drawing.Color.FromName(_color);
        Color unityColor = new Color32(c.R, c.G, c.B, c.A);
        lobbyPlayers[playerID].playerColor = unityColor;
        lobbyPlayers[playerID].playerBar.GetComponent<PlayerBarManager>().SwitchColor(unityColor);
    }

    public void UpdateName(int playerID, string name)
    {
        lobbyPlayers[playerID].playerName = name;
        lobbyPlayers[playerID].playerBar.GetComponent<PlayerBarManager>().SetName(name);
    }

    public void SetBarOwnership(int playerID, bool isPlayer)
    {
        lobbyPlayers[playerID].playerBar.GetComponent<PlayerBarManager>().SetOwnership(isPlayer);
    }

    public void SetServerOwner(bool isOwner)
    {
        startRoomButton.gameObject.SetActive(isOwner);
    }

    public void SetServerIP(string ip)
    {
        serverIP.text = "IP: " + ip;
    }

    public void SetServerPort(int port)
    {
        serverPort.text = "Port: " + port.ToString();
    }
}
