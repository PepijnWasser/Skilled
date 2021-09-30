using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected GameObject player;
    MouseCursor mouseCursor;
    protected Camera playerCamera;
    protected float range;

    protected virtual void Start()
    {
        GameManager.playerMade += SetCam;
        mouseCursor = GameObject.FindObjectOfType<MouseCursor>();
    }

    protected virtual void OnDestroy()
    {
        GameManager.playerMade -= SetCam;
    }

    protected virtual void Update()
    {

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
