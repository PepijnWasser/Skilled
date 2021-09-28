using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


public class KeypadTask : Task
{
    public delegate void Damage(int amount);
    public static event Damage taskDealDamage;

    public string code;
    public bool isFocused;

    public CinemachineVirtualCamera TaskCam;
    public KeyPadCodeEnterer keypadCodeEnterer;


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
        TestFocus();
    }


    void TestFocus()
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) < playerRange && isFocused == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Focus();
            }
        }
        if (isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeFocus();
            }
        }
    }

    protected override void CompleteTask()
    {
        base.CompleteTask();
        DeFocus();
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
        if(keypadCodeEnterer.message == code)
        {
            CompleteTask();
        }
        else
        {
            keypadCodeEnterer.DisplayErrorMessage();
        }
    }

    void Focus()
    {
        isFocused = true;
        player.GetComponent<MeshRenderer>().enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        TaskCam.Priority = 12;
    }

    void DeFocus()
    {
        isFocused = false;
        player.GetComponent<MeshRenderer>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        TaskCam.Priority = 10;
        keypadCodeEnterer.DisplayWelcomeMessage();
    }

    protected override void TestTask()
    {

    }
}
