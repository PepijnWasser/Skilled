using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneController : Switchable
{
    public Animator craneAnimator;
    public Animator rowAnimator;

    public float moveTime;
    public float waitTime;

    bool on = true;

    public float secondCounter = 0;

    public Vector2 oldPos;
    public Vector2 targetPos;

    public override void TurnOff()
    {
        on = false;
    }

    public override void TurnOn()
    {
        on = true;
    }


    private void Update()
    {
        if (on)
        {
            secondCounter += Time.deltaTime;

            craneAnimator.Play("Base Layer.craneAnimation", 0, Mathf.Lerp(oldPos.x, targetPos.x, secondCounter / moveTime));
            rowAnimator.Play("Base Layer.rowAnimation", 0, Mathf.Lerp(oldPos.y, targetPos.y, secondCounter / moveTime));

            if (secondCounter > (moveTime + waitTime))
            {
                secondCounter = 0;

                oldPos.x = craneAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                oldPos.y = rowAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                targetPos = new Vector2(Random.Range((float)0, (float)1), Random.Range((float)0, (float)1));
            }

        }
    }
}
