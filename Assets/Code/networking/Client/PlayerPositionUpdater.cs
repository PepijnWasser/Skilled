using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionUpdater : MonoBehaviour
{
    float secondCounter = 0;
    public int updateFrequency;
    public GameState gameState;
    public GameObject player;
    Vector3 oldPos;
    Vector3 oldRot;

    void Update()
    {
        secondCounter += Time.deltaTime;
        if(secondCounter >= (float)1 / (float)updateFrequency)
        {
            secondCounter = 0;
            if(player != null)
            {
                if (player.transform.position != oldPos || player.transform.rotation.eulerAngles != oldRot)
                {
                    gameState.SendPlayerPosition(player.transform.position, player.transform.rotation.eulerAngles);
                    oldPos = player.transform.position;
                    oldRot = player.transform.rotation.eulerAngles;
                }
            }
        }
    }
}
