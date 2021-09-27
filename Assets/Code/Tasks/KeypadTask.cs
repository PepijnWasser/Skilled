using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadTask : Task
{
    public delegate void Damage(int amount);
    public static event Damage taskDealDamage;

    public string code;

    public override void InitializeTask()
    {
        base.InitializeTask();

        int first = Random.Range(0,10);
        int second = Random.Range(0, 10);
        int third = Random.Range(0, 10);

        code = first.ToString() + second.ToString() + third.ToString();
    }

    protected override void Update()
    {
        base.Update();
        TestDamage();
    }


    protected override void TestTask()
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) < playerRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CompleteTask();
            }
        }
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
}
