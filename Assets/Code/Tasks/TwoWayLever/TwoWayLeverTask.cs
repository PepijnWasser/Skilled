using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayLeverTask : Task
{
    public delegate void Damage(int amount);
    public static event Damage taskDealDamage;

    public int targetPosition = 0;

    public float validationTime;
    float secondCounterValidate = 0;

    public TwoWayLever lever;

    bool completed = false;

    private void Start()
    {
        lever = GetComponent<TwoWayLever>();
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

    public override void InitializeTask()
    {
        base.InitializeTask();

        bool doneChecking = false;

        while (!doneChecking)
        {
            targetPosition = Random.Range(0, 2);
            if (targetPosition != lever.currentPosition)
            {
                doneChecking = true;
            }
        }
    }

    protected override void CompleteTask()
    {
        base.CompleteTask();
        Debug.Log("task completed");
    }


    public override void TestDamage()
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
        if (lever.currentPosition == targetPosition)
        {
            secondCounterValidate += Time.deltaTime;
            if (secondCounter > validationTime)
            {
                completed = true;
            }
        }
        else
        {
            secondCounter = 0;
        }
    }
}
