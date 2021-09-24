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

        player.name = _name + " " +  playerID;
        if (!playerControlled)
        {
            player.GetComponent<PlayerPrefabManager>().DisablePlayerActivity();
        }
        else
        {
            player.name += " controlled";
            playerPositionUpdater.player = player;
        }
    }

    public void MovePlayer(int playerID, Vector3 position, Vector3 rotation)
    {
        characterDictionary[playerID].transform.position = position;
        characterDictionary[playerID].transform.rotation = Quaternion.Euler(rotation);
    }
}
