using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenTester : MonoBehaviour
{

    [SerializeField] private ConsoleRoom consoleRoomPrefab;
    [SerializeField] private SecGenTester sectionGeneratorPrefab;


    public List<Doorway> posibleStartingDoors = new List<Doorway>();

    List<Doorway> doorwaysFromThis = new List<Doorway>();
    public List<Doorway> availableDoorwaysFromThis = new List<Doorway>();

    List<SecGenTester> sectionGenerators = new List<SecGenTester>();
    List<Doorway> doorsWaysRemoved = new List<Doorway>();

    public delegate void Completed();
    public static event Completed OnCompletion;

    void Start()
    {
        Initizlize(1);
    }

    public void Initizlize(int _amountOfSectors)
    {
        ConsoleRoom consoleRoom = Instantiate(consoleRoomPrefab, this.transform);
        foreach (Doorway doorway in consoleRoom.doorways)
        {
            doorwaysFromThis.Add(doorway);
            availableDoorwaysFromThis.Add(doorway);
        }

        GenerateSection();
    }

    void GenerateSection()
    {
        UpdateAvailibleDoorList();
        SecGenTester sectionGenerator = Instantiate(sectionGeneratorPrefab, this.transform);
        Doorway startDoor = Extensions.RandomListItem(posibleStartingDoors);

        sectionGenerator.transform.position = startDoor.transform.position;

        sectionGenerator.startdoor = startDoor;
        sectionGenerator.Initialize(LayerMask.GetMask("Room"));

        sectionGenerator.StartGeneration();
    }

    void UpdateAvailibleDoorList()
    {
        posibleStartingDoors.Clear();
        if (sectionGenerators.Count > 0)
        {
            foreach (SecGenTester generator in sectionGenerators)
            {
                foreach (Doorway d in generator.GetAllOpenDoorways())
                {
                    posibleStartingDoors.Add(d);
                }
            }
        }

        if (availableDoorwaysFromThis.Count > 0)
        {
            foreach (Doorway d in availableDoorwaysFromThis)
            {
                posibleStartingDoors.Add(d);
            }
        }

        foreach (Doorway d in doorsWaysRemoved)
        {
            posibleStartingDoors.Remove(d);
        }
    }
}
