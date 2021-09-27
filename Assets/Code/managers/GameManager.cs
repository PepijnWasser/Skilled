using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    public PlayerPositionUpdater playerPositionUpdater;
    Dictionary<int, GameObject> characterDictionary = new Dictionary<int, GameObject>();

    public delegate void PlayerMade(GameObject player);
    public static event PlayerMade playerMade;

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
            playerPositionUpdater.playerNose = player.GetComponent<PlayerPrefabManager>().nose;

            playerMade?.Invoke(player);
        }
    }

    public void MovePlayer(int playerID, Vector3 position, Vector3 rotation, Vector3 noseRotation)
    {
        characterDictionary[playerID].transform.position = position;
        characterDictionary[playerID].transform.rotation = Quaternion.Euler(rotation);
        characterDictionary[playerID].GetComponent<PlayerPrefabManager>().nose.transform.rotation = Quaternion.Euler(noseRotation); 
    }
}
