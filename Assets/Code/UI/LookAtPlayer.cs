using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public GameObject player;

    private void Update()
    {
        if(player != null)
        {
            this.transform.LookAt(player.transform);
        }
        else
        {
            player = GameManager.playerCharacter;
        }
    }
}
