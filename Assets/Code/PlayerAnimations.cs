using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public float resetTime = 0.1f;

    Animator animator;
    float secondCounter = 0;
    int animationIndex = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayRunAnimation()
    {
        animationIndex = 2;
        animator.SetInteger("MovementPlayer", 2);
        secondCounter = 0;

    }

    public void PlayIdleAnimation()
    {
        animationIndex = 0;
        animator.SetInteger("MovementPlayer", 0);
    }

    private void Update()
    {
        if(animationIndex != 0)
        {
            secondCounter += Time.deltaTime;
            if(secondCounter > resetTime)
            {
                animationIndex = 0;
                secondCounter = 0;
                animator.SetInteger("MovementPlayer", 0);
            }
        }
    }
}
