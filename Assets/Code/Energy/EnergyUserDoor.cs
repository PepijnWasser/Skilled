using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUserDoor : EnergyUser
{
    Animator animator;

    public GameObject raycastOrigin;
    public int ID;

    bool currentState = false;
    bool targetState = false;


    protected override void Start()
    {
        animator = GetComponent<Animator>();
        base.Start();
    }

    protected override void TurnOn()
    {
        base.TurnOn();
        animator.SetInteger("DoorState", 1);
        targetState = true;


    }

    protected override void TurnOff()
    {
        base.TurnOff();
        animator.SetInteger("DoorState", 0);
        targetState = false;
    }

    private void Update()
    {
        if(targetState != currentState)
        {
            if(targetState == false)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(raycastOrigin.transform.position, Vector3.down, out raycastHit, 0.1f))
                {
                    if(raycastHit.collider.gameObject != this.gameObject)
                    {
                        animator.enabled = false;
                    }
                    else
                    {
                        animator.enabled = true;
                    }
                }
                else
                {
                    animator.enabled = true;
                }

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Closed"))
                {
                    currentState = targetState;
                }
            }
            else
            {
                if(animator.enabled == false)
                {
                    animator.enabled = true;
                }
            }
        }

        if(targetState == false)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Closed"))
            {
                currentState = targetState;
            }
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Opened"))
            {
                currentState = targetState;
            }
        }
    
    }
    
}
