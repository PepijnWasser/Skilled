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
            sectionGenerator.startdoor = startDoor;
            sectionGenerator.GenerateLevel();

            //sectionGenerators.Add(sectionGenerator);

            //UpdateAvailibleDoorList();

        }
        StopCoroutine("GenerateLevel");
    }

    void UpdateAvailibleDoorList()
    {
        availableDoorways.Clear();;
    }
}
