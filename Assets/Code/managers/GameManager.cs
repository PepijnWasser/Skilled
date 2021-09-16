using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    public void MakePlayerCharacter(bool playerControlled, Vector3 position, string _name)
    {
        GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);
        player.name = _name;
        if (!playerControlled)
        {
            player.GetComponent<PlayerPrefabManager>().DestroyCameras();
        }
    }

    public void UpdatePlayerPosition(Vector3 position)
    {

    }
}
