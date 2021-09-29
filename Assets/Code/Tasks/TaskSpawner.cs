using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Tasks/TaskSpawner")]
public class TaskSpawner : ScriptableObject
{
    public GameObject LeverPrefab;
    public GameObject KeypadPrefab;

    public int amountOfLeverssToSpawn;
    public int amountOfKeypadsToSpawn;

    List<ThreeWayLeverLocation> availibleLeverLocations;
    List<KeypadLocation> availibleKeypadLocations;


    public List<Task> SpawnTasks(Transform parent)
    {
        List<Task> tasks = new List<Task>();

        GetAvailibleLocations();
        for(int i = 0; i < amountOfLeverssToSpawn; i++)
        {
            ThreeWayLeverLocation newlocation = Extensions.RandomListItem(availibleLeverLocations);
            availibleLeverLocations.Remove(newlocation);

            Task newTask = Instantiate(LeverPrefab, newlocation.transform.position, newlocation.transform.rotation * Quaternion.Euler(0, 180, 0), parent).GetComponent<Task>();
            tasks.Add(newTask);
        }

        for (int i = 0; i < amountOfKeypadsToSpawn; i++)
        {
            KeypadLocation newlocation = Extensions.RandomListItem(availibleKeypadLocations);
            availibleKeypadLocations.Remove(newlocation);

            Task newTask = Instantiate(KeypadPrefab, newlocation.transform.position, newlocation.transform.rotation, parent).GetComponent<Task>();
            tasks.Add(newTask);
        }

        return tasks;
    }

    void GetAvailibleLocations()
    {
        availibleLeverLocations = GameObject.FindObjectsOfType<ThreeWayLeverLocation>().ToList();
        availibleKeypadLocations = GameObject.FindObjectsOfType<KeypadLocation>().ToList();
    }
}
