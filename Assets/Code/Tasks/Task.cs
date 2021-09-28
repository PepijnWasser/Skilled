using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
    public delegate void Completed(Task taskCompleted);
    public static event Completed taskCompleted;

    public float playerRange;

    [SerializeField]
    protected float timeTillDamage;

    [SerializeField]
    protected int damageAmount;
    [SerializeField]
    protected float DamageRate;

    protected float secondCounter;

    public bool hasError = false;
    protected bool dealingDamage = false;

    [SerializeField]
    protected GameObject player;

    protected virtual void Start()
    {
        GameManager.playerMade += SetPlayer;
        SetPlayer(GameObject.FindObjectOfType<PlayerMovement>().gameObject);
    }

    protected virtual void OnDestroy()
    {
        GameManager.playerMade -= SetPlayer;
    }

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
        if (hasError)
        {
            TestTask();
        }
    }

    protected abstract void TestTask();
    

    void SetPlayer(GameObject _player)
    {
        player = _player;
    }
}
