using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject taskManagerPrefab;

    public GameObject energyManagerPrefab;
    public GameObject itemSpawnerPrefab;

    Dictionary<int, PlayerPrefabManager> characterDictionary = new Dictionary<int, PlayerPrefabManager>();

    List<PlayerSpawnLocation> availiblePlayerSpawnLocations = new List<PlayerSpawnLocation>();

    public static GameObject playerCharacter;

    public delegate void PlayerMade(GameObject player, Camera cam);
    public static event PlayerMade playerMade;

    public void MakePlayerCharacter(bool playerControlled, string _name, int playerID, string color)
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

        System.Drawing.Color c = System.Drawing.Color.FromName(color);
        Color unityColor = new Color32(c.R, c.G, c.B, c.A);
        manager.characterRenderer.material.color = unityColor;

        if (playerControlled)
        {
            manager.player.name += " controlled";
            manager.name += " controlled";

            manager.playerRotationScript.sensitivity = PlayerInfo.sensitivity;

            playerCharacter = manager.player;

            Destroy(manager.playerName.transform.parent.gameObject);

            playerMade?.Invoke(manager.player, manager.playerCam);
        }
        else
        {
            manager.DisablePlayerActivity();
            manager.player.GetComponent<Rigidbody>().useGravity = false;
            manager.player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            manager.playerName.text = _name;
        }
    }

    public void MovePlayer(int playerID, Vector3 position, Vector3 rotation, Vector3 targetRotation)
    {
        if (characterDictionary[playerID] != null)
        {
            Debug.Log("moving player " + playerID);

            PlayerPrefabManager manager = characterDictionary[playerID];

            if (position != manager.player.transform.position)
            {
                manager.playerAnimator.PlayRunAnimation();
            }

            manager.player.transform.position = position;
            manager.player.transform.rotation = Quaternion.Euler(rotation);
            manager.lookTarget.transform.rotation = Quaternion.Euler(targetRotation);
        }
    }

    public void RemovePlayerCharacter(int playerID)
    {
        Destroy(characterDictionary[playerID].gameObject);
        characterDictionary.Remove(playerID);
    }


    public void MakeWorldObjects(bool playerIsLeader, int maxErrors, int tasksOfTypeToSpawn)
    {
        GameObject taskManager = Instantiate(taskManagerPrefab);
        taskManager.GetComponent<TaskSpawner>().tasksOfTypeToSpawn = tasksOfTypeToSpawn;

        if (!playerIsLeader)
        {
            Destroy(taskManager.GetComponent<TaskManager>());
        }
        else
        {
            taskManager.GetComponent<TaskManager>().maxErrors = maxErrors;
        }

        GameObject energyManager = Instantiate(energyManagerPrefab);
        GameObject itemManager = Instantiate(itemSpawnerPrefab);
    }

    //gets all positions a player can spawn
    void GetAvailibleSpawnLocations()
    {
        availiblePlayerSpawnLocations.Clear();
        availiblePlayerSpawnLocations = GameObject.FindObjectsOfType<PlayerSpawnLocation>().ToList();
    }  
}
