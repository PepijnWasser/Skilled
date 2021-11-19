using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    
    [SerializeField] private ConsoleRoom consoleRoomPrefab;
    [SerializeField] private List<SectionGenerator> sectionGeneratorPrefabs;

    List<SectionGenerator> availibleSectionGenerators;

    [HideInInspector] int amountOfSectors = 0;
    int amountOfSectorsSpawned = 0;

    public List<Doorway> posibleStartingDoors = new List<Doorway>();

    List<Doorway> doorwaysFromThis = new List<Doorway>();
    public List<Doorway> availableDoorwaysFromThis = new List<Doorway>();

    List<SectionGenerator> sectionGenerators = new List<SectionGenerator>();

    SectionGenerator lastSectionGenerated;
    Doorway lastDoorUsed;

    List<Doorway> doorsWaysRemoved = new List<Doorway>();

    bool generating;

    public delegate void Completed();
    public static event Completed onCompletion;

    void Start()
    {
        SectionGenerator.OnCompletion += FinishSectionGeneration;
        availibleSectionGenerators = sectionGeneratorPrefabs;
    }

    private void OnDestroy()
    {
        SectionGenerator.OnCompletion -= FinishSectionGeneration;
    }

    public void Initizlize(int _amountOfSectors)
    {
        amountOfSectors = _amountOfSectors;

        ConsoleRoom consoleRoom = Instantiate(consoleRoomPrefab, this.transform);
        foreach (Doorway doorway in consoleRoom.doorways)
        {
            doorwaysFromThis.Add(doorway);
            availableDoorwaysFromThis.Add(doorway);
        }
    }

    void GenerateLevel()
    {
        if (amountOfSectors > 0)
        {
            if (amountOfSectorsSpawned < amountOfSectors)
            {
                if (!generating)
                {
                    generating = true;
                    GenerateSection();
                }
            }
        }
    }

    void GenerateSection()
    {
        UpdateAvailibleDoorList();
        SectionGenerator sectionGenerator = Instantiate(GetSectionGenerator(), this.transform);
        Doorway startDoor = Extensions.RandomListItem(posibleStartingDoors);

        sectionGenerator.transform.position = startDoor.transform.position;

        sectionGenerator.startdoor = startDoor;
        sectionGenerator.Initialize(LayerMask.GetMask("Room"));

        sectionGenerator.StartGeneration();
        lastSectionGenerated = sectionGenerator;
    }

    void UpdateAvailibleDoorList()
    {
        posibleStartingDoors.Clear();
        if(sectionGenerators.Count > 0)
        {
            foreach (SectionGenerator generator in sectionGenerators)
            {
                foreach (Doorway d in generator.GetAllOpenDoorways())
                {
                    posibleStartingDoors.Add(d);
                }
            }
        }

        if(availableDoorwaysFromThis.Count > 0)
        {
            foreach (Doorway d in availableDoorwaysFromThis)
            {
                posibleStartingDoors.Add(d);
            }
        }

        foreach(Doorway d in doorsWaysRemoved)
        {
            posibleStartingDoors.Remove(d);
        }
    }

    List<Doorway> GetAllDoorsList()
    {
        List<Doorway> doorways = new List<Doorway>();
        foreach (SectionGenerator g in sectionGenerators)
        {
            foreach(Doorway d in g.allDoorways)
            {
                doorways.Add(d);
            }
        }
        foreach(Doorway d in doorwaysFromThis)
        {
            doorways.Add(d);
        }

        return doorways;
    }

    void RemoveDoorsInSameSpace()
    {
        List<Doorway> allDoorways = GetAllDoorsList();

        foreach (Doorway doorway in allDoorways)
        {
            foreach (Doorway doorway2 in allDoorways)
            {
                //remove doorways if position is the same
                if ((doorway.transform.position - doorway2.transform.position).magnitude < 0.001 && doorway != doorway2)
                {
                    doorway.gameObject.SetActive(false);
                    //doorway.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    doorsWaysRemoved.Add(doorway);

                    doorway2.gameObject.SetActive(false);
                    //doorway2.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    doorsWaysRemoved.Add(doorway2);
                }
            }
        }
    }

    void FinishSectionGeneration(bool finished)
    {
        if (finished)
        {
            amountOfSectorsSpawned += 1;

            if (availableDoorwaysFromThis.Count > 0 && availableDoorwaysFromThis.Contains(lastDoorUsed))
            {
                availableDoorwaysFromThis.Remove(lastDoorUsed);
            }
            sectionGenerators.Add(lastSectionGenerated);
            availibleSectionGenerators.Remove(lastSectionGenerated);

            RemoveDoorsInSameSpace();
            UpdateAvailibleDoorList();

            if(amountOfSectorsSpawned >= amountOfSectors)
            {
                onCompletion?.Invoke();
            }
        }
        else
        {
            Destroy(lastSectionGenerated.gameObject);
        }
        generating = false;
    }

    private void Update()
    {
        GenerateLevel();
    }

    SectionGenerator GetSectionGenerator()
    {
        if(availibleSectionGenerators.Count > 0)
        {
            SectionGenerator generator = Extensions.RandomListItem(availibleSectionGenerators);
            return generator;
        }
        return null;

    }
}
