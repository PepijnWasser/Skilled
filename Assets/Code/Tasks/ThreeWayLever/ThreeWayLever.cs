using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeWayLever : MonoBehaviour
{
    public float playerRange;

    ThreeWayLeverInteractable leverInteractable;

    GameObject player;

    Animator animator;

    [HideInInspector]
    public int currentPosition;

    private void Start()
    {
        GameManager.playerMade += SetPlayer;

        animator = GetComponent<Animator>();
        leverInteractable = GetComponent<ThreeWayLeverInteractable>();

        currentPosition = Random.Range(1, 4);
        animator.SetInteger("Stance", currentPosition);
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
        if (Vector3.Distance(player.transform.position, this.transform.position) < playerRange)
        {
            if (Input.GetKeyDown(KeyCode.E) && leverInteractable.lookingAtTarget)
            {
                currentPosition += 1;
                if(currentPosition > 3)
                {
                    currentPosition = 1;
                }
                animator.SetInteger("Stance", currentPosition);
            }
        }
    }

    void SetPlayer(GameObject _player)
    {
        player = _player;
    }
}
