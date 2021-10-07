using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIcon : MonoBehaviour
{
    public Vector3 targetPos;

    public GameObject player;

    private void Update()
    {
        this.transform.position = new Vector3(player.transform.position.x, targetPos.y, player.transform.position.z);
    }
}
