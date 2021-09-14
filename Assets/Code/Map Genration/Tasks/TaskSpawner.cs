using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TaskSpawner : MonoBehaviour
{
    public GameObject TerminalPrefab;
    public GameObject KeypadPrefab;

    public int amountOfTerminalsToSpawn;
    public int amountOfKeypadsToSpawn;

    List<TerminalLocation> availibleTerminalLocations;
    List<KeypadLocation> availibleKeypadLocations;

    void Start()
    {
        MapGenerator.OnCompletion += SpawnTasks;
        SpawnTasks();
    }

    private void OnDestroy()
    {
        MapGenerator.OnCompletion -= SpawnTasks;
    }

    void SpawnTasks()
    {
        GetAvailibleLocations();
        for(int i = 0; i < amountOfTerminalsToSpawn; i++)
        {
            TerminalLocation newlocation = Extensions.RandomListItem(availibleTerminalLocations);
            availibleTerminalLocations.Remove(newlocation);

            Instantiate(TerminalPrefab, newlocation.transform.position, newlocation.transform.rotation);
        }

        for (int i = 0; i < amountOfKeypadsToSpawn; i++)
        {
            KeypadLocation newlocation = Extensions.RandomListItem(availibleKeypadLocations);
            availibleKeypadLocations.Remove(newlocation);

            Instantiate(KeypadPrefab, newlocation.transform.position, newlocation.transform.rotation);
        }
    }

    void GetAvailibleLocations()
    {
        availibleTerminalLocations = GameObject.FindObjectsOfType<TerminalLocation>().ToList();
        availibleKeypadLocations = GameObject.FindObjectsOfType<KeypadLocation>().ToList();
    }
}
