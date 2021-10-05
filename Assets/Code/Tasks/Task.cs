using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    public delegate void Completed(Task taskCompleted);
    public static event Completed taskCompleted;

    [SerializeField]
    protected float timeTillDamage;

    [SerializeField]
    protected int damageAmount;
    [SerializeField]
    protected float DamageRate;

    protected float secondCounter;

    public bool hasError = false;
    protected bool dealingDamage = false;

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
}
