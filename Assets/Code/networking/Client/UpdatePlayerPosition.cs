using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerPosition : MonoBehaviour
{
    float secondCounter = 0;
    public int updateFrequency;
    public GameState gameState;
    public GameObject player;

    void Update()
    {
        secondCounter += Time.deltaTime;
        if(secondCounter >= 1 / updateFrequency)
        {
            gameState.SendPlayerPosition(player.transform.position);
        }
    }
}
