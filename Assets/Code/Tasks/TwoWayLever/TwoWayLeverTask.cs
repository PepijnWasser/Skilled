using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayLeverTask : Task
{
    public delegate void Damage(int amount);
    public static event Damage taskDealDamage;

    [SerializeField]
    private int targetPosition = 0;

    [SerializeField]
    private float validationTime;
    private float secondCounterValidate = 0;

    public TwoWayLever lever;

    private bool completed = false;

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

    //get a new random targetPosition
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
    }

    //test damage
    public override void TestDamage()
    {
        if (hasError)
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
    }

    //check if the lever is in the correct position for validationTime seconds
    public void ValidatePosition()
    {
        if (lever.currentPosition == targetPosition)
        {
            secondCounterValidate += Time.deltaTime;
            if (secondCounterValidate > validationTime)
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

    public int GetTargetPosition()
    {
        return targetPosition;
    }

    public void SetTargetPosition(int _position)
    {
        targetPosition = _position;
    }
}
