using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalTask : Task
{
    public delegate void Damage(int amount);
    public static event Damage taskDealDamage;

    public override void InitializeTask()
    {
        base.InitializeTask();
    }

    protected override void CompleteTask()
    {
        base.CompleteTask();
    }

    protected override void TestTask()
    {
        
    }

    protected override void Update()
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
}
