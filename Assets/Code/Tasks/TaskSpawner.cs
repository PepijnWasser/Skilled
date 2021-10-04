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

    private void Start()
    {
        
    }

    public List<Task> SpawnTasks(Transform parent)
    {
        List<Task> tasks = new List<Task>();

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

            TwoWayLever twoWayLever = newObject.GetComponent<TwoWayLever>();
            twoWayLever.name = twoWayLeverNamer.GetName();

            TwoWayLeverTask task = newObject.GetComponent<TwoWayLeverTask>();

            tasks.Add(task);
        }

        for (int i = 0; i < amountOfThreeLeverssToSpawn; i++)
        {
            ThreeWayLeverLocation newlocation = Extensions.RandomListItem(availibleThreeWayLeverLocations);
            availibleThreeWayLeverLocations.Remove(newlocation);

            GameObject newObject = Instantiate(ThreeWayLeverPrefab, newlocation.transform.position, newlocation.transform.rotation, parent);

            ThreeWayLever threeWayLever = newObject.GetComponent<ThreeWayLever>();
            threeWayLever.name = threeWayLeverNamer.GetName();

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
