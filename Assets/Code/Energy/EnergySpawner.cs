using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySpawner : MonoBehaviour
{
    public GameObject doorPrefab;

    public int amountOfDoorsToSpawn;


    List<EnergyDoorLocation> availibleEnergyDoorLocation;

    public delegate void EnergyUsersSpawned();
    public static event EnergyUsersSpawned energyUsersSpawned;

    static int newID = 0;

    //foreach type of task, spawn the given amount of that task. Give it a name and id, and set the location
    public void SpawnEnergyUsers(Transform parent)
    {
        GetAvailibleLocations();

        for (int i = 0; i < amountOfDoorsToSpawn; i++)
        {
            EnergyDoorLocation newlocation = Extensions.RandomListItem(availibleEnergyDoorLocation);
            availibleEnergyDoorLocation.Remove(newlocation);

            GameObject newObject = Instantiate(doorPrefab, newlocation.transform.position, newlocation.transform.rotation, parent);
            EnergyUserDoor manager = newObject.GetComponent<EnergyUserDoor>();

            manager.ID = i;
        }

        energyUsersSpawned?.Invoke();
    }

    //gets all positions a task can spawn
    void GetAvailibleLocations()
    {
        availibleEnergyDoorLocation = new List<EnergyDoorLocation>(GameObject.FindObjectsOfType<EnergyDoorLocation>());
    }

    public static  int getNewID()
    {
        newID += 1;
        return newID;
    }
}
