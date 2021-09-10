using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    
    [SerializeField] private ConsoleRoom consoleRoomPrefab;
    [SerializeField] private SectionGenerator sectionGeneratorPrefab;

    public int amountOfSectors;

    public List<Doorway> availableDoorways = new List<Doorway>();

    List<SectionGenerator> sectionGenerators = new List<SectionGenerator>();


    void Start()
    {
        StartCoroutine("GenerateLevel");
    }

    IEnumerator GenerateLevel()
    {
        WaitForFixedUpdate interval = new WaitForFixedUpdate();
        yield return interval;

        ConsoleRoom consoleRoom = Instantiate(consoleRoomPrefab, this.transform);
        foreach(Doorway doorway in consoleRoom.doorways)
        {
            availableDoorways.Add(doorway);
        }

        for(int i = 0; i < amountOfSectors; i++)
        {
            SectionGenerator sectionGenerator = Instantiate(sectionGeneratorPrefab, this.transform);
            Doorway startDoor = Extensions.RandomListItem(availableDoorways);

            sectionGenerator.transform.position = startDoor.transform.position;

            sectionGenerator.Initialize(LayerMask.GetMask("Room"));
            sectionGenerator.GenerateLevel();

            sectionGenerators.Add(sectionGenerator);

            UpdateAvailibleDoorList();
            RemoveDoorsInSameSpace();


        }
        StopCoroutine("GenerateLevel");
    }

    void UpdateAvailibleDoorList()
    {
        availableDoorways.Clear();
        GetAllDoors();
        foreach(Doorway doorway in doorsUsedAsStartPoint)
        {
            availableDoorways.Remove(doorway);
        }
    }

    List<Doorway> GetAllDoors()
    {
        List<Doorway> allDoorways = new List<Doorway>();
        foreach (SectionGenerator sectionGenerator in sectionGenerators)
        {
            foreach (Doorway doorway in sectionGenerator.availableDoorways)
            {
                allDoorways.Add(doorway);
            }
            foreach (Doorway doorway in sectionGenerator.as)
            {
                allDoorways.Add(doorway);
            }
        }
        foreach (Doorway doorway in startDoorways)
        {
            availableDoorways.Add(doorway);
        }

        return allDoorways;
    }

    void RemoveDoorsInSameSpace()
    {
        List<Doorway> allAvailableDoorways = GetAllDoors();

        foreach (Doorway doorway in allAvailableDoorways)
        {
            foreach (Doorway doorway2 in allAvailableDoorways)
            {
                //remove doorways if position is the same
                if ((doorway.transform.position - doorway2.transform.position).magnitude < 0.001 && doorway != doorway2)
                {
                    //doorway.gameObject.SetActive(false);
                    doorway.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    availableDoorways.Remove(doorway);

                    //doorway2.gameObject.SetActive(false);
                    doorway2.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    availableDoorways.Remove(doorway2);
                }
            }
        }
    }
    */
}
