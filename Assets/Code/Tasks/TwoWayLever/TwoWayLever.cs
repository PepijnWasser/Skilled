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

    public int leverID = 0;

    public delegate void LeverPulled(int ID, int newPos);
    public static event LeverPulled leverPulled;

    protected void Awake()
    {
        GameManager.playerMade += SetPlayer;
        GameState.updateTwoWayLeverPos += UpdatePos;
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
        GameState.updateTwoWayLeverPos -= UpdatePos;
    }

    //if we have a player check if the lever is pulled
    private void Update()
    {
        if(player != null)
        {
            TestPull();
        }
    }

    //check if the lever is pulled and play the correct position
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

            leverPulled?.Invoke(leverID, currentPosition);
        }
    }

    void SetPlayer(GameObject _player, Camera cam)
    {
        player = _player;
    }

    //updates the lever position when another player pulled it
    void UpdatePos(UpdateTwoWayLeverPositionMessage message)
    { 
        if(message.leverID == leverID)
        {
            currentPosition = message.leverPosition;
            animator.SetInteger("position", currentPosition);
        }
    }
}
