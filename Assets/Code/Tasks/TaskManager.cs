using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public TaskSpawner taskSpawner;

    public int maxErrors;
    
    public Vector2 timeBetweenErrors;
    public float timeToNextError;
    float secondCounter = 0;

    List<Task> tasksWithoutErrors = new List<Task>();
    List<Task> tasksWithErrors = new List<Task>();

    public delegate void Spawning(Task taskSpawned);
    public static event Spawning taskHasError;

    void Start()
    {
        MapGenerator.OnCompletion += SpawnTasks;
        Task.taskCompleted += FinishTask;

        timeToNextError = timeBetweenErrors.y;
    }

    private void OnDestroy()
    {
        MapGenerator.OnCompletion -= SpawnTasks;
        Task.taskCompleted -= FinishTask;
    }

    void SpawnTasks()
    {
        tasksWithoutErrors = taskSpawner.SpawnTasks(this.transform);
        secondCounter = 0;
    }

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
                        taskHasError?.Invoke(newTask);

                        secondCounter = 0;
                        timeToNextError = Random.Range(timeBetweenErrors.x, timeBetweenErrors.y);
                    }
                    else
                    {
                        //Debug.Log("too few tasks");
                    }
                }
            }       
        }
    }

    void FinishTask(Task task)
    {
        tasksWithErrors.Remove(task);
        tasksWithoutErrors.Add(task);
    }
}
