using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUserDoor : EnergyUser
{
    Animator animator;

    public GameObject raycastOrigin;
    public BoxCollider collider;

    public bool targetState = false;


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
        if(targetState == false)
        {
            Collider[] hitColliders = Physics.OverlapSphere(raycastOrigin.transform.position, 2);

            bool touchingPlayer = false;
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.layer == 13)
                {
                    touchingPlayer = true;
                }
            }

            if (touchingPlayer)
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

    }
    
}
