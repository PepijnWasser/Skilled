using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Tasks/TaskSpawner")]
public class TaskSpawner : ScriptableObject
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

    public List<Task> SpawnTasks(Transform parent)
    {
        List<Task> tasks = new List<Task>();

        GetAvailibleLocations();

        for (int i = 0; i < amountOfTwoWayLeversToSpawn; i++)
        {
            TwoWayLeverLocation newlocation = Extensions.RandomListItem(availibleTwoWayLeverLocations);
            availibleTwoWayLeverLocations.Remove(newlocation);

            Task newTask = Instantiate(TwoWayLeverPrefab, newlocation.transform.position, newlocation.transform.rotation * Quaternion.Euler(0, 180, 0), parent).GetComponent<Task>();
            tasks.Add(newTask);
        }

        for (int i = 0; i < amountOfThreeLeverssToSpawn; i++)
        {
            ThreeWayLeverLocation newlocation = Extensions.RandomListItem(availibleThreeWayLeverLocations);
            availibleThreeWayLeverLocations.Remove(newlocation);

            Task newTask = Instantiate(ThreeWayLeverPrefab, newlocation.transform.position, newlocation.transform.rotation * Quaternion.Euler(0, 180, 0), parent).GetComponent<Task>();
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
        availibleThreeWayLeverLocations = GameObject.FindObjectsOfType<ThreeWayLeverLocation>().ToList();
        availibleTwoWayLeverLocations = GameObject.FindObjectsOfType<TwoWayLeverLocation>().ToList();
        availibleKeypadLocations = GameObject.FindObjectsOfType<KeypadLocation>().ToList();
    }
}
