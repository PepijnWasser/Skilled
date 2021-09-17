using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerPosition : MonoBehaviour
{
    float secondCounter = 0;
    public int updateFrequency;
    public GameState gameState;
    [HideInInspector] public GameObject player;
    Vector3 oldPos;

    void Update()
    {
        secondCounter += Time.deltaTime;
        if(secondCounter >= 1 / updateFrequency)
        {
            if(player.transform.position != oldPos)
            {
                secondCounter = 0;
                gameState.SendPlayerPosition(player.transform.position);
                oldPos = player.transform.position;
            }
        }
    }
}
