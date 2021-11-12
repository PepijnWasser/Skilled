using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject taskManagerPrefab;

    Dictionary<int, PlayerPrefabManager> characterDictionary = new Dictionary<int, PlayerPrefabManager>();

    List<PlayerSpawnLocation> availiblePlayerSpawnLocations = new List<PlayerSpawnLocation>();

    public GameObject playerCharacter;

    public delegate void PlayerMade(GameObject player, Camera cam);
    public static event PlayerMade playerMade;

    public void MakePlayerCharacter(bool playerControlled, Vector3 position, string _name, int playerID)
    {
        if(availiblePlayerSpawnLocations.Count == 0 || availiblePlayerSpawnLocations == null)
        {
            GetAvailibleSpawnLocations();
        }

        PlayerSpawnLocation newlocation = Extensions.RandomListItem(availiblePlayerSpawnLocations);
        availiblePlayerSpawnLocations.Remove(newlocation);

        GameObject newPlayer = Instantiate(playerPrefab, newlocation.transform.position + new Vector3(0, 0, 0), newlocation.transform.rotation);
        PlayerPrefabManager manager = newPlayer.GetComponent<PlayerPrefabManager>();

        characterDictionary.Add(playerID, manager);

        manager.player.name = _name + " " + playerID;
        manager.name = _name + " " + playerID;
        manager.iconText.text = _name;

        if (playerControlled)
        {
            manager.player.name += " controlled";
            manager.name += " controlled";

            manager.playerRotationScript.sensitivity = PlayerInfo.sensitivity;

            playerMade?.Invoke(manager.player, manager.playerCam);

            playerCharacter = manager.player;
        }
        else
        {
            manager.DisablePlayerActivity();
            manager.player.GetComponent<Rigidbody>().useGravity = false;
            manager.player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void MovePlayer(int playerID, Vector3 position, Vector3 rotation, Vector3 noseRotation)
    {
        PlayerPrefabManager manager = characterDictionary[playerID];
        manager.player.transform.position = position;
        manager.player.transform.rotation = Quaternion.Euler(rotation);
        manager.nose.transform.rotation = Quaternion.Euler(noseRotation);
    }

    public void RemovePlayerCharacter(int playerID)
    {
        Destroy(characterDictionary[playerID].gameObject);
        characterDictionary.Remove(playerID);
    }


    public void MakeTaskmanager(bool playerIsLeader)
    {
        GameObject manager = Instantiate(taskManagerPrefab);
        if (!playerIsLeader)
        {
            manager.GetComponent<TaskManager>().enabled = false;
        }
    }

    //gets all positions a player can spawn
    void GetAvailibleSpawnLocations()
    {
        availiblePlayerSpawnLocations.Clear();
        availiblePlayerSpawnLocations = GameObject.FindObjectsOfType<PlayerSpawnLocation>().ToList();
    }  
}
