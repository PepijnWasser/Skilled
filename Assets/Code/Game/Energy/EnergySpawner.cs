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
        SpawnEnergyUsers();
    }

    //foreach type of task, spawn the given amount of that task. Give it a name and id, and set the location
    public void SpawnEnergyUsers()
    {
        GetAvailibleLocations();

        for (int i = 0; i < amountOfDoorsToSpawn; i++)
        {
            EnergyDoorLocation newlocation = Extensions.RandomListItem(availibleEnergyDoorLocation);
            availibleEnergyDoorLocation.Remove(newlocation);

            GameObject newObject = Instantiate(doorPrefab, newlocation.transform.position, newlocation.transform.rotation, newlocation.transform);
        }

        energyUsersSpawned?.Invoke();
    }

    //gets all positions a task can spawn
    void GetAvailibleLocations()
    {
        availibleEnergyDoorLocation = GameObject.FindObjectsOfType<EnergyDoorLocation>().ToList();
        RemoveLocationsInSameSpace(availibleEnergyDoorLocation);
    }

    void RemoveLocationsInSameSpace(List<EnergyDoorLocation> locations)
    {
        List<EnergyDoorLocation> locationsToRemove = new List<EnergyDoorLocation>();

        foreach (EnergyDoorLocation location in locations)
        {
            foreach (EnergyDoorLocation location2 in locations)
            {
                if (location != location2)
                {
                    if (Vector3.Distance(location.transform.position, location2.transform.position) < 1 && locationsToRemove.Contains(location2) == false)
                    {
                        locationsToRemove.Add(location);
                    }
                }
            }
        }

        foreach(EnergyDoorLocation location in locationsToRemove)
        {
            availibleEnergyDoorLocation.Remove(location);
        }
    }

    public static  int getNewID()
    {
        newID += 1;
        return newID;
    }
}
