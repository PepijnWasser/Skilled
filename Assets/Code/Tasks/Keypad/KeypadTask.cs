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

    public delegate void Validate(string code, int id);
    public static event Validate validateCode;

    public delegate void SetKeypadMessage(int id, bool correct);
    public static event SetKeypadMessage codeChecked;

    public string code = "";

    private void Awake()
    {
        GameState.testKeypadCode += TestKeypadCode;
        GameState.setKeypadOutcomeMessage += SetStatus;
    }

    private void Start()
    {
        keyPad = GetComponent<Keypad>();
        keypadCodeEnterer = GetComponent<KeypadCodeEnterer>();
    }

    private void OnDestroy()
    {
        GameState.testKeypadCode -= TestKeypadCode;
        GameState.setKeypadOutcomeMessage -= SetStatus;
    }

    public override void InitializeTask()
    {
        base.InitializeTask();

        int first = Random.Range(1,10);
        int second = Random.Range(1, 10);
        int third = Random.Range(1, 10);

        code = first.ToString() + second.ToString() + third.ToString();
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
        validateCode?.Invoke(keypadCodeEnterer.message, keyPad.keypadID);
    }

    public override int GetID()
    {
        return keyPad.keypadID;
    }

    void SetStatus(int _id, bool outCome)
    {
        if(keyPad.keypadID == _id)
        {
            if(outCome == true)
            {
                keypadCodeEnterer.DisplayWelcomeMessage();
                correctAudio.Play();

                if (ServerConnectionData.isOwner)
                {
                    CompleteTask();
                }
            }
            else
            {
                keypadCodeEnterer.DisplayErrorMessage();
                badAudio.Play();
            }
        }
    }

    void TestKeypadCode(string _code, int _id)
    {
        if (ServerConnectionData.isOwner)
        {
            if (keyPad.keypadID == _id)
            {
                if (hasError)
                {
                    if (code == _code.ToString())
                    {
                        codeChecked?.Invoke(keyPad.keypadID, true);
                    }
                    else
                    {
                        codeChecked?.Invoke(keyPad.keypadID, false);
                    }
                }
                else
                {
                    codeChecked?.Invoke(keyPad.keypadID, false);
                }
            }
        }
    }
}
