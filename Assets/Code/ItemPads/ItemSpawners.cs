using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemSpawners : MonoBehaviour
{
    public List<GameObject> smallObjects;
    public List<GameObject> mediumObjects;
    public List<GameObject> bigObjects;

    List<SmallItemLocation> availibleSmallLocations;
    List<MediumItemLocation> availibleMediumLocations;
    List<BigItemLocation> availibleBigLocations;

    private void Start()
    {
        SpawnItems();
    }

    public void SpawnItems()
    {
        GetAvailibleLocations();

        foreach(SmallItemLocation location in availibleSmallLocations)
        {
            int random = Random.Range(1, 4);
            if(random == 1)
            {
                GameObject newObject = Extensions.RandomListItem(smallObjects);
                Instantiate(newObject, location.transform.position, location.transform.rotation);
            }
        }
        foreach (MediumItemLocation location in availibleMediumLocations)
        {
            int random = Random.Range(1, 4);
            if (random == 1)
            {
                GameObject newObject = Extensions.RandomListItem(mediumObjects);
                Instantiate(newObject, location.transform.position, location.transform.rotation);
            }
        }
        foreach (BigItemLocation location in availibleBigLocations)
        {
            int random = Random.Range(1, 4);
            if (random == 1)
            {
                GameObject newObject = Extensions.RandomListItem(bigObjects);
                Instantiate(newObject, location.transform.position, location.transform.rotation);
            }
        }
    }

    void GetAvailibleLocations()
    {
        availibleSmallLocations = GameObject.FindObjectsOfType<SmallItemLocation>().ToList();
        availibleMediumLocations = GameObject.FindObjectsOfType<MediumItemLocation>().ToList();
        availibleBigLocations = GameObject.FindObjectsOfType<BigItemLocation>().ToList();
    }
}
