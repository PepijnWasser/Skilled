using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Tasks/TaskSpawner")]
public class TaskSpawner : ScriptableObject
{
    public GameObject TerminalPrefab;
    public GameObject KeypadPrefab;

    public int amountOfTerminalsToSpawn;
    public int amountOfKeypadsToSpawn;

    List<TerminalLocation> availibleTerminalLocations;
    List<KeypadLocation> availibleKeypadLocations;


    public List<Task> SpawnTasks(Transform parent)
    {
        List<Task> tasks = new List<Task>();

        GetAvailibleLocations();
        for(int i = 0; i < amountOfTerminalsToSpawn; i++)
        {
            TerminalLocation newlocation = Extensions.RandomListItem(availibleTerminalLocations);
            availibleTerminalLocations.Remove(newlocation);

            Task newTask = Instantiate(TerminalPrefab, newlocation.transform.position, newlocation.transform.rotation, parent).GetComponent<Task>();
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
        availibleTerminalLocations = GameObject.FindObjectsOfType<TerminalLocation>().ToList();
        availibleKeypadLocations = GameObject.FindObjectsOfType<KeypadLocation>().ToList();
    }
}
