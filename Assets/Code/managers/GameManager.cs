using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    public UpdatePlayerPosition playerPositionUpdater;
    Dictionary<int, GameObject> characterDictionary = new Dictionary<int, GameObject>();

    public void MakePlayerCharacter(bool playerControlled, Vector3 position, string _name, int playerID)
    {
        GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);

        characterDictionary.Add(playerID, player);
        Debug.Log(playerID);

        player.name = _name;
        if (!playerControlled)
        {
            player.GetComponent<PlayerPrefabManager>().DestroyCameras();
        }
        else
        {
            playerPositionUpdater.player = player;
        }
    }

    public void MovePlayer(int playerID, Vector3 position)
    {
        Debug.Log(playerID + characterDictionary[playerID].transform.name);
        characterDictionary[playerID].transform.position = position;
    }
}
