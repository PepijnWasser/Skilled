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

    KeyPadNamer keypadNamer;

    public List<Task> SpawnTasks(Transform parent)
    {
        List<Task> tasks = new List<Task>();
        keypadNamer = GameObject.FindObjectOfType<KeyPadNamer>();


        GetAvailibleLocations();

        for (int i = 0; i < amountOfTwoWayLeversToSpawn; i++)
        {
            TwoWayLeverLocation newlocation = Extensions.RandomListItem(availibleTwoWayLeverLocations);
            availibleTwoWayLeverLocations.Remove(newlocation);

            GameObject newObject = Instantiate(TwoWayLeverPrefab, newlocation.transform.position, newlocation.transform.rotation, parent);

            TwoWayLeverTask task = newObject.GetComponent<TwoWayLeverTask>();
            tasks.Add(task);
        }

        for (int i = 0; i < amountOfThreeLeverssToSpawn; i++)
        {
            ThreeWayLeverLocation newlocation = Extensions.RandomListItem(availibleThreeWayLeverLocations);
            availibleThreeWayLeverLocations.Remove(newlocation);

            GameObject newObject = Instantiate(ThreeWayLeverPrefab, newlocation.transform.position, newlocation.transform.rotation, parent);

            ThreeWayLeverTask task = newObject.GetComponent<ThreeWayLeverTask>();
            tasks.Add(task);
        }

        for (int i = 0; i < amountOfKeypadsToSpawn; i++)
        {
            KeypadLocation newlocation = Extensions.RandomListItem(availibleKeypadLocations);
            availibleKeypadLocations.Remove(newlocation);

            GameObject newObject = Instantiate(KeypadPrefab, newlocation.transform.position, newlocation.transform.rotation, parent);

            Keypad keypad = newObject.GetComponent<Keypad>();
            keypad.name = keypadNamer.GetName();

            KeypadTask task = newObject.GetComponent<KeypadTask>();

            tasks.Add(task);
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
