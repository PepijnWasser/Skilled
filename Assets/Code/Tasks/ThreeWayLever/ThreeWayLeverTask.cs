using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThreeWayLeverTask : Task
{
    public delegate void Damage(int amount);
    public static event Damage taskDealDamage;

    public int position = 0;

    public float validationTime;
    float secondCounterValidate = 0;

    ThreeWayLever lever;

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
            position = Random.Range(1, 4);
            if(position != lever.currentPosition)
            {
                doneChecking = true;
            }
        }

    }

    protected override void Update()
    {
        base.Update();
        if (hasError)
        {
            TestDamage();
            ValidatePosition();
        }
    }



    protected override void CompleteTask()
    {
        base.CompleteTask();
        Debug.Log("task completed");
    }


    void TestDamage()
    {

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
        if(lever.currentPosition == position)
        {
            secondCounterValidate += Time.deltaTime;
            if(secondCounter > validationTime)
            {
                CompleteTask();
            }
        }
        else
        {
            secondCounter = 0;
        }
    }
}
