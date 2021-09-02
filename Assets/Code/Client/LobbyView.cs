using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyView : MonoBehaviour
{
    public GameObject playerBarPrefab;
    public GameObject verticalLayoutGroupPlayers;

    Dictionary<int, PlayerData> lobbyPlayers = new Dictionary<int, PlayerData>();


    public void AddPlayer(int playerID, string playerColor, string playerName, bool isPlayer)
    {
        PlayerData newPlayerData = new PlayerData(playerColor, playerName);
        lobbyPlayers.Add(playerID, newPlayerData);
        
        GameObject newPlayerBar = Instantiate(playerBarPrefab, verticalLayoutGroupPlayers.transform);
        newPlayerData.AddPlayerBar(newPlayerBar);

        UpdatePlayerColor(playerID, playerColor);
        UpdateName(playerID, playerName);
        SetOwnership(playerID, isPlayer);

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

    public void SetOwnership(int playerID, bool isPlayer)
    {
        lobbyPlayers[playerID].playerBar.GetComponent<PlayerBarManager>().SetOwnership(isPlayer);
    }
}
