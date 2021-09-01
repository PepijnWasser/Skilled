using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePlayerInfo : MonoBehaviour
{
    public GameObject inputField;
    public PlayerInfo playerInfo;

    public void StoreName()
    {
        string name = inputField.GetComponent<Text>().text;
        playerInfo.playerName = name;
    }
}
