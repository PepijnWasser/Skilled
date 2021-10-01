using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeWayLever : MonoBehaviour
{
    Interactable interactable;

    GameObject player;

    Animator animator;

    [HideInInspector]
    public int currentPosition;

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>().gameObject;

        animator = GetComponent<Animator>();
        interactable = GetComponent<Interactable>();

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
        if (Input.GetKeyDown(KeyCode.E) && interactable.lookingAtTarget)
        {
            currentPosition += 1;
            if (currentPosition > 3)
            {
                currentPosition = 1;
            }
            animator.SetInteger("Stance", currentPosition);
        }
    }
}
