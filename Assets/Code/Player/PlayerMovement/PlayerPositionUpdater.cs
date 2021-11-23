using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionUpdater : MonoBehaviour
{
    float secondCounter = 0;
    public int updateFrequency;

    GameState gameState;
    GameObject player;
    public GameObject lookTarget;

    Vector3 oldPos;
    Vector3 oldRot;
    Vector3 oldRotTarget;

    private void Awake()
    {
        gameState = GameObject.FindObjectOfType<GameState>();
        player = this.gameObject;
    }

    //sends the new player position/rotation through udp
    void Update()
    {
        secondCounter += Time.deltaTime;
        if(secondCounter >= (float)1 / (float)updateFrequency)
        {
            secondCounter = 0;
            if(player != null)
            {
                if (player.transform.position != oldPos || player.transform.rotation.eulerAngles != oldRot || lookTarget.transform.rotation.eulerAngles != oldRotTarget)
                {
                    gameState.SendPlayerPosition(player.transform.position, player.transform.rotation.eulerAngles, lookTarget.transform.rotation.eulerAngles);
                    oldPos = player.transform.position;
                    oldRot = player.transform.rotation.eulerAngles;
                    oldRotTarget = lookTarget.transform.rotation.eulerAngles;
                }
            }
        }
    }
}
