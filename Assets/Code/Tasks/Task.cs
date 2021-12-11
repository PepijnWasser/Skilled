using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    public delegate void Completed(Task taskCompleted);
    public static event Completed taskCompleted;

    //time in seconds until task starts doing damage to the station
    [SerializeField]
    protected float timeTillDamage;

    //amount of damage the task does each time
    [SerializeField]
    protected int damageAmount;
    //how fast the task does damage once it starts doing damage
    [SerializeField]
    protected float DamageRate;

    public float secondCounter;

    public bool hasError = false;
    public bool dealingDamage = false;

    //name of the task
    public string taskName;


    public virtual void InitializeTask()
    {
        hasError = true;
    }

    protected virtual void CompleteTask()
    {
        hasError = false;
        dealingDamage = false;
        secondCounter = 0;
        taskCompleted?.Invoke(this);
    }

    protected virtual void Update()
    {

    }

    public virtual void TestDamage()
    {

    }

    public virtual int GetID()
    {
        return 0;
    }
}
