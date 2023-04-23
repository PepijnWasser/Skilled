using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [HideInInspector]
    public int maxErrors;
    
    [SerializeField]
    Vector2 timeBetweenErrors;
    private float timeToNextError;
    private float secondCounter = 0;

    [SerializeField]
    List<Task> tasksWithoutErrors = new List<Task>();
    [SerializeField]
    List<Task> tasksWithErrors = new List<Task>();

    public delegate void Spawning(Task taskSpawned, int taskID);
    public static event Spawning taskHasError;

    private void Awake()
    {
        Task.taskCompleted += FinishTask;
        TaskSpawner.allTasksSpawned += SetTasks;
    }

    void Start()
    {
        timeToNextError = timeBetweenErrors.y;
    }

    private void OnDestroy()
    {
        Task.taskCompleted -= FinishTask;
        TaskSpawner.allTasksSpawned -= SetTasks;
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

    //add the task to the correct list
    void FinishTask(Task task)
    {
        tasksWithErrors.Remove(task);
        tasksWithoutErrors.Add(task);
    }

    //sets the availible tasks
    void SetTasks(List<Task> tasks)
    {
        tasksWithoutErrors = tasks;
    }
}
