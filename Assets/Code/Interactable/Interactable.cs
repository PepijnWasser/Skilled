using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected GameObject player;
    MouseCursor mouseCursor;
    protected Camera playerCamera;

    [SerializeField]
    float range;

    protected virtual void Start()
    {
        GameManager.playerMade += SetCam;
        SetCam(GameObject.FindObjectOfType<PlayerMovement>().gameObject);
        mouseCursor = GameObject.FindObjectOfType<MouseCursor>();
    }

    protected virtual void OnDestroy()
    {
        GameManager.playerMade -= SetCam;
    }

    protected virtual void Update()
    {
        RaycastHit hit;

        if (Vector3.Distance(player.transform.position, this.transform.position) < range)
        {
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
            {
                OnHit(hit);
            }
        }
    }

    protected virtual void OnHit(RaycastHit hit)
    {
        Debug.DrawRay(player.transform.position, player.transform.forward * hit.distance, Color.yellow);
        mouseCursor.ShowCursor();

    }

    void SetCam(GameObject _player)
    {
        player = _player;
        playerCamera = player.GetComponentInChildren<Camera>();
    }
}
