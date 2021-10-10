using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject taskManagerPrefab;

    public PlayerPositionUpdater playerPositionUpdater;
    Dictionary<int, PlayerPrefabManager> characterDictionary = new Dictionary<int, PlayerPrefabManager>();

    public delegate void PlayerMade(GameObject player, Camera cam);
    public static event PlayerMade playerMade;


    public void MakePlayerCharacter(bool playerControlled, Vector3 position, string _name, int playerID)
    {
        GameObject newPlayer = Instantiate(playerPrefab, position, Quaternion.identity);
        PlayerPrefabManager manager = newPlayer.GetComponent<PlayerPrefabManager>();

        characterDictionary.Add(playerID, manager);

        manager.player.name = _name + " " + playerID;
        manager.name = _name + " " + playerID;

        if (playerControlled)
        {
            manager.player.name += " controlled";
            manager.name += " controlled";

            playerPositionUpdater.player = manager.player;
            playerPositionUpdater.playerNose = manager.nose;

            playerMade?.Invoke(manager.player, manager.camera);
        }
        else
        {
            manager.DisablePlayerActivity();
        }
    }

    public void MovePlayer(int playerID, Vector3 position, Vector3 rotation, Vector3 noseRotation)
    {
        PlayerPrefabManager manager = characterDictionary[playerID];
        manager.player.transform.position = position;
        manager.player.transform.rotation = Quaternion.Euler(rotation);
        manager.nose.transform.rotation = Quaternion.Euler(noseRotation);
    }

    public void MakeTaskmanager(bool playerIsLeader)
    {
        GameObject manager = Instantiate(taskManagerPrefab);
        if (!playerIsLeader)
        {
            manager.GetComponent<TaskManager>().enabled = false;
        }
    }
}
