using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject managersParent;
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
        PlayerPrefabManager playerPrefabManager = newPlayer.GetComponent<PlayerPrefabManager>();

        characterDictionary.Add(playerID, playerPrefabManager);

        playerPrefabManager.player.name = _name + " " + playerID;
        playerPrefabManager.name = _name + " " + playerID;
        playerPrefabManager.iconText.text = _name;

        System.Drawing.Color c = System.Drawing.Color.FromName(color);
        Color unityColor = new Color32(c.R, c.G, c.B, c.A);
        playerPrefabManager.characterRenderer.material.color = unityColor;

        if (playerControlled)
        {
            playerPrefabManager.player.name += " controlled";
            playerPrefabManager.name += " controlled";

            playerPrefabManager.playerRotationScript.sensitivity = PlayerInfo.sensitivity;

            playerCharacter = playerPrefabManager.player;

            Destroy(playerPrefabManager.playerName.transform.parent.gameObject);

            playerMade?.Invoke(playerPrefabManager.player, playerPrefabManager.playerCam);
        }
        else
        {
            playerPrefabManager.DisablePlayerActivity();
            playerPrefabManager.player.GetComponent<Rigidbody>().useGravity = false;
            playerPrefabManager.player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            playerPrefabManager.playerName.text = _name;
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

    public void DeleteMusicPlayer()
    {
        GameObject musicPlayer = GameObject.FindObjectOfType<MusicManager>().gameObject;
        Destroy(musicPlayer);
    }


    public void MakeWorldObjects(bool playerIsLeader, int maxErrors, int tasksOfTypeToSpawn)
    {
        GameObject taskManager = Instantiate(taskManagerPrefab, managersParent.transform);
        taskManager.GetComponent<TaskSpawner>().tasksOfTypeToSpawn = tasksOfTypeToSpawn;

        if (!playerIsLeader)
        {
            Destroy(taskManager.GetComponent<TaskManager>());
        }
        else
        {
            taskManager.GetComponent<TaskManager>().maxErrors = maxErrors;
        }

        GameObject energyManager = Instantiate(energyManagerPrefab, managersParent.transform);
        GameObject itemManager = Instantiate(itemSpawnerPrefab, managersParent.transform);
    }

    //gets all positions a player can spawn
    void GetAvailibleSpawnLocations()
    {
        availiblePlayerSpawnLocations.Clear();
        availiblePlayerSpawnLocations = GameObject.FindObjectsOfType<PlayerSpawnLocation>().ToList();
    }  
}
