using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected GameObject player;
    public bool lookingAtTarget;
    MouseCursor mouseCursor;
    protected Camera playerCamera;
    public float range;
    public GameObject body;


    protected virtual void Awake()
    {
        GameManager.playerMade += SetCam;
    }

    protected virtual void Start()
    {
        mouseCursor = GameObject.FindObjectOfType<MouseCursor>();
    }


    protected virtual void Update()
    {
        if (player != null)
        {
            lookingAtTarget = false;
            RaycastHit hit;

            float dist = Vector3.Distance(player.transform.position, this.transform.position);
            if (dist < range)
            {
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
                {
                    OnHit(hit);
                }
            }
        }
    }

    protected virtual void OnHit(RaycastHit hit)
    {
        if (body == hit.transform.gameObject)
        {
            Debug.DrawRay(player.transform.position, player.transform.forward * hit.distance, Color.yellow);
            lookingAtTarget = true;
            mouseCursor.ShowCursor();
        }
    }

    void SetCam(GameObject _player, Camera cam)
    {
        player = _player;
        playerCamera = cam;
    }
}
