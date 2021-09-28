using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public TaskSpawner taskSpawner;

    public int maxErrors;
    
    public Vector2 timeBetweenErrors;
    float timeToNextError;
    float secondCounter = 0;

    public List<Task> tasksWithoutErrors = new List<Task>();
    public List<Task> tasksWithErrors = new List<Task>();

    void Start()
    {
        MapGenerator.OnCompletion += SpawnTasks;
        Task.taskCompleted += FinishTask;

        SpawnTasks();

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
    }

    private void Update()
    {
        secondCounter += Time.deltaTime;

        if(secondCounter > timeToNextError)
        {
            if(tasksWithoutErrors.Count > 0)
            {
                if(tasksWithErrors.Count < maxErrors)
                {
                    Task newTask = Extensions.RandomListItem(tasksWithoutErrors);
                    tasksWithErrors.Add(newTask);
                    tasksWithoutErrors.Remove(newTask);
                    newTask.InitializeTask();

                    secondCounter = 0;
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
