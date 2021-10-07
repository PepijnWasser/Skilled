using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


public class KeypadTask : Task
{
    [HideInInspector]
    public Keypad keyPad;
    KeypadCodeEnterer keypadCodeEnterer;

    public delegate void Damage(int amount);
    public static event Damage taskDealDamage;

    public string code = "";

    private void Start()
    {
        keyPad = GetComponent<Keypad>();
        keypadCodeEnterer = GetComponent<KeypadCodeEnterer>();
    }

    public override void InitializeTask()
    {
        base.InitializeTask();

        int first = Random.Range(1,10);
        int second = Random.Range(1, 10);
        int third = Random.Range(1, 10);

        code = first.ToString() + second.ToString() + third.ToString();
    }

    protected override void CompleteTask()
    {
        base.CompleteTask();
        keyPad.DeFocus();
        Debug.Log("task completed");
    }


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

    public void ValidateCode()
    {
        if(keypadCodeEnterer == null)
        {
            keypadCodeEnterer = GetComponent<KeypadCodeEnterer>();
        }

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

    public override int getID()
    {
        return keyPad.keypadID;
    }
}
