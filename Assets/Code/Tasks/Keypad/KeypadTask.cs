using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


public class KeypadTask : Task
{
    Keypad keyPad;
    KeypadCodeEnterer keypadCodeEnterer;

    public delegate void Damage(int amount);
    public static event Damage taskDealDamage;

    public string code = "";


    public override void InitializeTask()
    {
        base.InitializeTask();

        keyPad = GetComponent<Keypad>();
        keypadCodeEnterer = GetComponent<KeypadCodeEnterer>();

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



    protected override void CompleteTask()
    {
        base.CompleteTask();
        keyPad.DeFocus();
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

    public void ValidateCode()
    {
        if (hasError)
        {
            if (keypadCodeEnterer.message == code)
            {
                CompleteTask();
            }
            else
            {
                keypadCodeEnterer.DisplayErrorMessage();
            }
        }
        else
        {
            keypadCodeEnterer.DisplayErrorMessage();
        }
    }
}
