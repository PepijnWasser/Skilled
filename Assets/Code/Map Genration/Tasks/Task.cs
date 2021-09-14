using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    public delegate void Completed(bool succeeded);
    public static event Completed OnCompletion;

    public abstract void CompleteTask();

    public abstract void TestTask();
}
