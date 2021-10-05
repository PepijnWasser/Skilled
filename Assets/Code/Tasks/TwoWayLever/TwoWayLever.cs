using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayLever : MonoBehaviour
{
    Interactable interactable;

    GameObject player;

    Animator animator;

    [HideInInspector]
    public int currentPosition;


    protected void Awake()
    {
        GameManager.playerMade += SetPlayer;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        interactable = GetComponent<Interactable>();

        currentPosition = Random.Range(0, 2);
        animator.SetInteger("position", currentPosition);
    }

    private void OnDestroy()
    {
        GameManager.playerMade -= SetPlayer;
    }

    private void Update()
    {
        if(player != null)
        {
            TestPull();
        }
    }

    void TestPull()
    { 
        if (Input.GetKeyDown(KeyCode.E) && interactable.lookingAtTarget)
        {
            currentPosition += 1;
            if (currentPosition > 1)
            {
                currentPosition = 0;
            }
            animator.SetInteger("position", currentPosition);
            Debug.Log(currentPosition);
        }
    }

    void SetPlayer(GameObject _player, Camera cam)
    {
        player = _player;
    }
}
