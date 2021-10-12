using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TaskSpawner : MonoBehaviour
{
    public GameObject TwoWayLeverPrefab;
    public GameObject ThreeWayLeverPrefab;
    public GameObject KeypadPrefab;

    public int amountOfTwoWayLeversToSpawn;
    public int amountOfThreeLeverssToSpawn;
    public int amountOfKeypadsToSpawn;

    List<TwoWayLeverLocation> availibleTwoWayLeverLocations;
    List<ThreeWayLeverLocation> availibleThreeWayLeverLocations;
    List<KeypadLocation> availibleKeypadLocations;

    KeyPadNamer keypadNamer;
    TwoWayLeverNamer twoWayLeverNamer;
    ThreeWayLeverNamer threeWayLeverNamer;

    public delegate void TasksSpawned(List<Task> tasks);
    public static event TasksSpawned allTasksSpawned;

    private void Start()
    {
        SpawnTasks(this.transform);
    }

    //foreach type of task, spawn the given amount of that task. Give it a name and id, and set the location
    public void SpawnTasks(Transform parent)
    {
        List<Task> tasksSpawned = new List<Task>();
        keypadNamer = GameObject.FindObjectOfType<KeyPadNamer>();
        twoWayLeverNamer = GameObject.FindObjectOfType<TwoWayLeverNamer>();
        threeWayLeverNamer = GameObject.FindObjectOfType<ThreeWayLeverNamer>();

        keypadNamer.Initialize();
        twoWayLeverNamer.Initialize();
        threeWayLeverNamer.Initialize();


        GetAvailibleLocations();
        
        for (int i = 0; i < amountOfTwoWayLeversToSpawn; i++)
        {
            TwoWayLeverLocation newlocation = Extensions.RandomListItem(availibleTwoWayLeverLocations);
            availibleTwoWayLeverLocations.Remove(newlocation);

            GameObject newObject = Instantiate(TwoWayLeverPrefab, newlocation.transform.position, newlocation.transform.rotation, parent);
            TwoWayLeverPrefabManager manager = newObject.GetComponent<TwoWayLeverPrefabManager>();

            manager.lever.leverID = i;

            string name = twoWayLeverNamer.GetName();
            manager.gameObject.name = name;
            manager.leverTask.taskName = name;
            manager.taskName.text = name;

            tasksSpawned.Add(manager.leverTask);
        }

        for (int i = 0; i < amountOfThreeLeverssToSpawn; i++)
        {
            ThreeWayLeverLocation newlocation = Extensions.RandomListItem(availibleThreeWayLeverLocations);
            availibleThreeWayLeverLocations.Remove(newlocation);

            GameObject newObject = Instantiate(ThreeWayLeverPrefab, newlocation.transform.position, newlocation.transform.rotation, parent);
            ThreeWayLeverPrefabManager manager = newObject.GetComponent<ThreeWayLeverPrefabManager>();

            manager.lever.leverID = i;

            string name = threeWayLeverNamer.GetName();
            manager.lever.gameObject.name = name;
            manager.leverTask.taskName = name;
            manager.taskName.text = name;

            tasksSpawned.Add(manager.leverTask);
        }

        for (int i = 0; i < amountOfKeypadsToSpawn; i++)
        {
            KeypadLocation newlocation = Extensions.RandomListItem(availibleKeypadLocations);
            availibleKeypadLocations.Remove(newlocation);

            GameObject newObject = Instantiate(KeypadPrefab, newlocation.transform.position, newlocation.transform.rotation, parent);
            KeypadPrefabManager manager = newObject.GetComponent<KeypadPrefabManager>();

            manager.keypad.keypadID = i;

            string name = keypadNamer.GetName();
            manager.keypad.gameObject.name = name;
            manager.keypadTask.taskName = name;
            manager.iconText.text = name;
            manager.codeEnterer.SetWelcomeMessage(name);

            tasksSpawned.Add(manager.keypadTask);
        }

        allTasksSpawned?.Invoke(tasksSpawned);
    }

    //gets all positions a task can spawn
    void GetAvailibleLocations()
    {
        availibleThreeWayLeverLocations = GameObject.FindObjectsOfType<ThreeWayLeverLocation>().ToList();
        availibleTwoWayLeverLocations = GameObject.FindObjectsOfType<TwoWayLeverLocation>().ToList();
        availibleKeypadLocations = GameObject.FindObjectsOfType<KeypadLocation>().ToList();
    }
}
