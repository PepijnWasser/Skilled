using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUserDoor : EnergyUser
{
    Animator animator;

    public GameObject raycastOrigin;
    public int ID;

    public bool currentState = false;
    public bool targetState = false;


    protected virtual void Start()
    {
       // base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void TurnOn()
    {
        base.TurnOn();
        animator.SetInteger("DoorState", 1);
        targetState = true;


    }

    protected override void TurnOff()
    {
      //  base.TurnOff();
        animator.SetInteger("DoorState", 0);
        targetState = false;
    }

    private void Update()
    {
        if (targetState == true)
        {
            animator.SetInteger("DoorState", 1);
        }
        else
        {
            animator.SetInteger("DoorState", 0);
        }




        if(targetState != currentState)
        {
            if(targetState == false)
            {
                Debug.Log("testing");
                RaycastHit raycastHit;
                if (Physics.Raycast(raycastOrigin.transform.position, Vector3.down, out raycastHit, 10))
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

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Closed") == false)
                {
                    currentState = targetState;
                }
            }
        }
    }
}
