using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnergySpawner : MonoBehaviour
{
    public GameObject doorPrefab;

    public int amountOfDoorsToSpawn;


    List<EnergyDoorLocation> availibleEnergyDoorLocation;

    public delegate void EnergyUsersSpawned();
    public static event EnergyUsersSpawned energyUsersSpawned;

    static int newID = 0;

    private void Start()
    {
        SpawnEnergyUsers(this.transform);
    }

    //foreach type of task, spawn the given amount of that task. Give it a name and id, and set the location
    public void SpawnEnergyUsers(Transform parent)
    {
        GetAvailibleLocations();

        for (int i = 0; i < amountOfDoorsToSpawn; i++)
        {
            EnergyDoorLocation newlocation = Extensions.RandomListItem(availibleEnergyDoorLocation);
            availibleEnergyDoorLocation.Remove(newlocation);

            GameObject newObject = Instantiate(doorPrefab, newlocation.transform.position, newlocation.transform.rotation, parent);
        }

        energyUsersSpawned?.Invoke();
    }

    //gets all positions a task can spawn
    void GetAvailibleLocations()
    {
        availibleEnergyDoorLocation = GameObject.FindObjectsOfType<EnergyDoorLocation>().ToList();
    }

    public static  int getNewID()
    {
        newID += 1;
        return newID;
    }
}
