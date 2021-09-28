using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TerminalTask : Task
{
    public delegate void Damage(int amount);
    public static event Damage taskDealDamage;

    public int position = 0;

    float secondCounterValidate = 0;

    Terminal terminal;

    private void Start()
    {
        terminal = GetComponent<Terminal>();
    }

    public override void InitializeTask()
    {
        base.InitializeTask();

        bool doneChecking = false;

        while (!doneChecking)
        {
            position = Random.Range(1, 4);
            if(position != terminal.currentPosition)
            {
                doneChecking = true;
            }
        }

    }

    protected override void Update()
    {
        base.Update();
        TestDamage();
        ValidatePosition();
    }



    protected override void CompleteTask()
    {
        base.CompleteTask();
        Debug.Log("task completed");
    }


    void TestDamage()
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

    public void ValidatePosition()
    {

    }


    protected override void TestTask()
    {

    }
}
