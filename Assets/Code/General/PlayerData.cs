using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public Color playerColor;
    public string playerName;
    public GameObject playerBar;

    public PlayerData(string _playerColor, string PlayerName)
    {
        System.Drawing.Color c = System.Drawing.Color.FromName(_playerColor);
        playerColor = new Color32(c.R, c.G, c.B, c.A);
    }

    public void AddPlayerBar(GameObject _playerBar)
    {
        playerBar = _playerBar;
    }

}
