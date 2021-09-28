using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public float playerRange;

    TerminalInteractable terminalInteractable;

    GameObject player;

    Animator animator;

    [HideInInspector]
    public int currentPosition = 1;

    private void Start()
    {
        GameManager.playerMade += SetPlayer;
        SetPlayer(GameObject.FindObjectOfType<PlayerMovement>().gameObject);

        animator = GetComponent<Animator>();
        terminalInteractable = GetComponent<TerminalInteractable>();
    }

    private void Update()
    {
        TestPull();
    }

    void TestPull()
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) < playerRange)
        {
            if (Input.GetKeyDown(KeyCode.E) && terminalInteractable.lookingAtTarget)
            {
                currentPosition += 1;
                if(currentPosition > 3)
                {
                    currentPosition = 1;
                }
                animator.SetInteger("Stance", currentPosition);
                Debug.Log(currentPosition);
            }
        }
    }

    void SetPlayer(GameObject _player)
    {
        player = _player;
    }
}
