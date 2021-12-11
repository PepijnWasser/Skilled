using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThreeWayLeverTask : Task
{
    public delegate void Damage(int amount);
    public static event Damage taskDealDamage;

    public int targetPosition = 0;

    public float validationTime;
    float secondCounterValidate = 0;

    public ThreeWayLever lever;

    bool completed = false;

    private void Start()
    {
        lever = GetComponent<ThreeWayLever>();
    }

    public override void InitializeTask()
    {
        base.InitializeTask();

        bool doneChecking = false;

        while (!doneChecking)
        {
            targetPosition = Random.Range(1, 4);
            if(targetPosition != lever.currentPosition)
            {
                doneChecking = true;
            }
        }

    }

    protected override void Update()
    {
        base.Update();
        if (completed)
        {
            CompleteTask();
            completed = false;
        }
    }


    protected override void CompleteTask()
    {
        base.CompleteTask();
        Debug.Log("task completed");
    }


    public override void TestDamage()
    {
        Debug.Log("3wl");
        secondCounter += Time.deltaTime;
        if (dealingDamage)
        {
            if (secondCounter > DamageRate)
            {
                secondCounter = 0;
                taskDealDamage?.Invoke(damageAmount);
            }
        }
        else
        {
            if (secondCounter > timeTillDamage)
            {
                dealingDamage = true;
            }
        }

    }

    public void ValidatePosition()
    {
        if(lever.currentPosition == targetPosition)
        {
            secondCounterValidate += Time.deltaTime;
            if(secondCounterValidate > validationTime)
            {
                completed = true;
            }
        }
        else
        {
            secondCounterValidate = 0;
        }
    }

    public override int GetID()
    {
        return lever.leverID;
    }
}
