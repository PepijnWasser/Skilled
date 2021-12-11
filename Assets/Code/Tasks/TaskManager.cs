using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [HideInInspector]
    public int maxErrors;
    
    public Vector2 timeBetweenErrors;
    public float timeToNextError;
    float secondCounter = 0;

    public List<Task> tasksWithoutErrors = new List<Task>();
    public List<Task> tasksWithErrors = new List<Task>();

    public delegate void Spawning(Task taskSpawned, int taskID);
    public static event Spawning taskHasError;

    private void Awake()
    {
        Task.taskCompleted += FinishTask;
        TaskSpawner.allTasksSpawned += getTasks;
    }

    void Start()
    {
        timeToNextError = timeBetweenErrors.y;
    }

    private void OnDestroy()
    {
        Task.taskCompleted -= FinishTask;
        TaskSpawner.allTasksSpawned -= getTasks;
    }

    //if there is a task availible, and we need to spawn a task, spawn a random task from list
    private void Update()
    {
        secondCounter += Time.deltaTime;

        if(secondCounter > timeToNextError)
        {
            if(tasksWithErrors != null && tasksWithoutErrors != null)
            {
                if (tasksWithErrors.Count < maxErrors)
                {
                    if (tasksWithoutErrors.Count > 0)
                    {
                        Task newTask = Extensions.RandomListItem(tasksWithoutErrors);
                        tasksWithErrors.Add(newTask);
                        tasksWithoutErrors.Remove(newTask);
                        newTask.InitializeTask();
                        taskHasError?.Invoke(newTask, newTask.GetID());

                        secondCounter = 0;
                        timeToNextError = Random.Range(timeBetweenErrors.x, timeBetweenErrors.y);
                    }
                    else
                    {
                        Debug.Log("too few tasks");
                    }
                }
            }       
        }
        TestTask();
    }

    //test if the current tasks are correct
    void TestTask()
    {
        foreach(Task task in tasksWithErrors)
        {
            task.TestDamage();
            if(task is TwoWayLeverTask)
            {
                TwoWayLeverTask twoWayTask = task as TwoWayLeverTask;
                twoWayTask.ValidatePosition();
            }
            else if (task is ThreeWayLeverTask)
            {
                ThreeWayLeverTask threeWayTask = task as ThreeWayLeverTask;
                threeWayTask.ValidatePosition();
            }
        }
    }

    void FinishTask(Task task)
    {
        tasksWithErrors.Remove(task);
        tasksWithoutErrors.Add(task);
    }

    void getTasks(List<Task> tasks)
    {
        tasksWithoutErrors = tasks;
    }
}
