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

    public AudioSource badAudio;
    public AudioSource correctAudio;


    public delegate void Damage(int amount);
    public static event Damage taskDealDamage;

    public delegate void Validate(int code, int id);
    public static event Validate validateCode;

    public string code = "";

    bool isServerOwner = true;

    private void Awake()
    {
        GameState.makeKeypadTask += SetCode;
        GameState.testKeypadCode += TestKeypadCode;
    }

    private void Start()
    {
        keyPad = GetComponent<Keypad>();
        keypadCodeEnterer = GetComponent<KeypadCodeEnterer>();
    }

    private void OnDestroy()
    {
        GameState.makeKeypadTask += SetCode;
        GameState.testKeypadCode -= TestKeypadCode;
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
        if (isServerOwner)
        {
            base.CompleteTask();
            keyPad.DeFocus();
            Debug.Log(true);
        }
        else
        {
            Debug.Log(false);
            validateCode?.Invoke(int.Parse(keypadCodeEnterer.message), keyPad.keypadID);
        }
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
                correctAudio.Play();
            }
            else
            {
                keypadCodeEnterer.DisplayErrorMessage();
                badAudio.Play();
            }
        }
        else
        {
            keypadCodeEnterer.DisplayErrorMessage();
        }
    }

    public override int GetID()
    {
        return keyPad.keypadID;
    }

    void SetCode(KeypadTask task, int keypadID)
    {
        hasError = true;
        isServerOwner = false;
        if(keyPad != null)
        {
            if(keyPad.keypadID == keypadID)
            {
                code = task.code;
            }
        }
    }

    void TestKeypadCode(int _code, int _id)
    {
        if(keyPad.keypadID == _id)
        {
            if(code == _code.ToString())
            {
                CompleteTask();
            }
        }
    }
}
