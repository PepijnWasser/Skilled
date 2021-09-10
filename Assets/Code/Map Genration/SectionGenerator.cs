using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System;

using Random = UnityEngine.Random;

public class SectionGenerator : MonoBehaviour
{
	[SerializeField] private bool spawnStartRoom = true;
	[SerializeField] private ConsoleRoom consoleRoomPrefab;

	public List<BuildingRoom> roomPrefabs = new List<BuildingRoom>();

	public Vector2 minMaxRooms = new Vector2(3, 10);
	public List<Doorway> allDoorways = new List<Doorway>();
	public List<Doorway> availableDoorways = new List<Doorway>();

	ConsoleRoom consoleRoom;
	private Doorway startdoor;


	List<BuildingRoom> placedRooms = new List<BuildingRoom>();

	LayerMask roomLayerMask;
	Color roomColor;

	BuildingRoom newestRoom;

	public void Initialize(LayerMask mask)
	{
		roomLayerMask = mask;
		StartCoroutine("GenerateLevel");
	}

	public void GenerateLevel()
	{
		roomColor = Random.ColorHSV();
		if (spawnStartRoom)
		{
			// Place start room
			PlaceConsoleRoom();
		}
		else
		{
			AddDoorwayToLists(startdoor);
		}

		//spawn between the min and max amount of rooms
		int iterations = Random.Range((int)minMaxRooms.x, (int)minMaxRooms.y);

		for (int i = 0; i < iterations; i++)
		{
			SpawnRoom();
			RemoveDoorsInSameSpace();
		}
	}

	void PlaceConsoleRoom()
	{
		// Instantiate room
		consoleRoom = Instantiate(consoleRoomPrefab);
		consoleRoom.transform.parent = this.transform;

		// Get doorways from current room and add them to the list of available doorways
		AddDoorwaysToLists(consoleRoom);

		// Position room
		consoleRoom.transform.position = Vector3.zero;
		consoleRoom.transform.rotation = Quaternion.identity;
	}

	void SpawnRoom()
    {
		bool roomSpawned = false;

		List<Doorway> existingDoorways = availableDoorways;
		List<Doorway> existingDoorwaysChecked = new List<Doorway>();
		Doorway existingDoorwayToCheck = Extensions.RandomListItem(existingDoorways);

		while (existingDoorwaysChecked.Count < existingDoorways.Count)
        {
			existingDoorwayToCheck = Extensions.Next(existingDoorways, existingDoorwayToCheck);
			existingDoorwaysChecked.Add(existingDoorwayToCheck);
			
            if (TestRooms(existingDoorwayToCheck))
            {
				roomSpawned = true;
				break;
            }
            else
            {
				availableDoorways.Remove(existingDoorwayToCheck);
				existingDoorwayToCheck.GetComponent<MeshRenderer>().material.color = Color.yellow;
            }
			
        }
        if (!roomSpawned)
        {
			Debug.Log("failed to spawn room");
        }
    }

	bool TestRooms(Doorway doorwayToFitOn)
    {
		List<BuildingRoom> roomsToCheck = roomPrefabs;
		List<BuildingRoom> roomsChecked = new List<BuildingRoom>();
		BuildingRoom roomToCheck = Extensions.RandomListItem(roomsToCheck);

		while (roomsChecked.Count < roomsToCheck.Count)
        {
			roomToCheck = Instantiate(Extensions.Next(roomsToCheck, roomToCheck),this.transform);
			roomsChecked.Add(roomToCheck);
			
			if(TestDoors(doorwayToFitOn, roomToCheck))
            {
				AddDoorwaysToLists(roomToCheck);
				roomToCheck.canSpawn = true;
				return true;
            }
            else
            {
				roomToCheck.canSpawn = false;
				Destroy(roomToCheck.gameObject);
            }
			
        }
		return false;
    }

	bool TestDoors(Doorway doorwayToFitOn, BuildingRoom buildingToTest)
    {
		List<Doorway> doorwaysOfRoom = buildingToTest.doorways;
		List<Doorway> doorwaysOfRoomChecked = new List<Doorway>();
		Doorway doorwayToCheck = Extensions.RandomListItem(doorwaysOfRoom);

		while(doorwaysOfRoomChecked.Count < doorwaysOfRoom.Count)
        {
			doorwayToCheck = Extensions.Next(doorwaysOfRoom, doorwayToCheck);
			doorwaysOfRoomChecked.Add(doorwayToCheck);
			if(TestDoor(doorwayToFitOn, buildingToTest, doorwayToCheck))
            {
				return true;
            }
        }
		return false;
    }

	bool TestDoor(Doorway doorwayToFitOn, BuildingRoom room, Doorway doorwayToTest)
	{
		PositionRoomAtDoorway(room, doorwayToTest, doorwayToFitOn);
		if (!CheckRoomOverlap(room))
		{
			room.GetComponentInChildren<Renderer>().material.color = roomColor;
			return true;
		}
		else
		{
			return false;
		}
	}

	void PositionRoomAtDoorway(BuildingRoom room, Doorway roomDoorway, Doorway targetDoorway)
	{
		// Reset room position and rotation
		room.transform.position = Vector3.zero;
		room.transform.rotation = Quaternion.identity;

		// Rotate room to match previous doorway orientation
		Vector3 targetDoorwayEuler = targetDoorway.transform.eulerAngles;
		targetDoorwayEuler.y = Mathf.Round(targetDoorwayEuler.y * 100.0f) * 0.01f;

		Vector3 roomDoorwayEuler = roomDoorway.transform.eulerAngles;
		float deltaAngle = Mathf.DeltaAngle(roomDoorwayEuler.y, targetDoorwayEuler.y);
		Quaternion RotationToCompensateDeltaAngle = Quaternion.AngleAxis(deltaAngle, Vector3.up);
		room.transform.rotation = (RotationToCompensateDeltaAngle * Quaternion.Euler(0, 180f, 0));

		Quaternion roomRotation = room.transform.rotation;
		roomRotation = Quaternion.Euler(0.0f, Mathf.Round(roomRotation.eulerAngles.y * 100.0f) * 0.01f, 0.0f);
		room.transform.rotation = roomRotation;


		// Position room
		Vector3 roomPositionOffset = roomDoorway.transform.position - room.transform.position;
		Vector3 newVec = targetDoorway.transform.position - roomPositionOffset;
		newVec = new Vector3(Mathf.Round(newVec.x * 100.0f) * 0.01f, Mathf.Round(newVec.y * 100.0f) * 0.01f, Mathf.Round(newVec.z * 100.0f) * 0.01f);

		room.transform.position = newVec;
	}


	bool CheckRoomOverlap(BuildingRoom room)
	{
		Bounds[] bounds = room.RoomBounds;
		for (int i = 0; i < bounds.Count(); i++)
		{
			bounds[i].center = room.transform.position;
			bounds[i].Expand(-0.1f);
		}
		Debug.Log(bounds[0].size);
		for (int i = 0; i < bounds.Count(); i++)
		{
			Collider[] colliders = Physics.OverlapBox(bounds[i].center, bounds[i].size / 2, room.transform.rotation, roomLayerMask);
			if (colliders.Length > 0)
			{
				// Ignore collisions with current room
				foreach (Collider c in colliders)
				{
					if (c.transform.parent.gameObject == room.gameObject)
					{
						continue;
					}
					else
					{
						return true;
					}
				}
			}

		}
		return false;
	}

	void RemoveDoorsInSameSpace()
	{
		List<Doorway> allAvailableDoorways = new List<Doorway>(availableDoorways);

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

	/*
	void ResetLevelGenerator()
	{
		Debug.Log("Reset level generator");

		StopCoroutine("GenerateLevel");

		// Delete all rooms
		if (consoleRoom)
		{
			DestroyImmediate(consoleRoom.gameObject);
		}

		foreach (BuildingRoom room in placedRooms)
		{
			if (room != null)
			{
				DestroyImmediate(room.gameObject);
			}
		}

		// Clear lists
		placedRooms.Clear();
		availableDoorways.Clear();
		removedDoorDoorDictionary.Clear();

		// Restart coroutine
		//------->>>>>EditorCoroutineUtility.StartCoroutineOwnerless(GenerateLevel());
	}
	*/
	/*
	void TestRoomRemoval()
	{
		Dictionary<Doorway, Doorway> removedDoorDoorCopy = new Dictionary<Doorway, Doorway>(removedDoorDoorDictionary);
		foreach (KeyValuePair<Doorway, Doorway> keyValue in removedDoorDoorCopy)
		{
			Doorway doorKey = null;
			Doorway doorValue = null;

			if (keyValue.Key != null)
			{
				doorKey = keyValue.Key;
			}

			if (keyValue.Value != null)
			{
				doorValue = keyValue.Value;
			}

			if (doorValue == null || doorKey == null)
			{
				if (doorValue == null)
				{
					Debug.Log("doorValue was nul");
					doorKey.gameObject.SetActive(true);
					availableDoorways.Add(doorKey);
				}
				else
				{
					Debug.Log("doorKey was nul");
					doorValue.gameObject.SetActive(true);
					availableDoorways.Add(doorValue);
				}
				removedDoorDoorDictionary.Remove(doorKey);
			}
		}
		List<Doorway> doorwaysToRemove = new List<Doorway>();
		foreach (Doorway doorway in availableDoorways)
		{
			if (doorway == null)
			{
				doorwaysToRemove.Add(doorway);
			}
		}
		foreach (Doorway doorway in doorwaysToRemove)
		{
			availableDoorways.Remove(doorway);
		}
		doorwaysToRemove.Clear();
	}
	*/
	/*
	public void GenerateNewLevel()
	{
		roomLayerMask = LayerMask.GetMask("Room");
		ResetLevelGenerator();
	}
	*/
	
	private void Update()
	{
		//TestRoomRemoval();
		if (Input.GetKeyDown(KeyCode.E))
		{
			SpawnRoom();
			RemoveDoorsInSameSpace();
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			if (newestRoom != null)
			{
				Destroy(newestRoom.gameObject);
			}
		}
	}
	

	void AddDoorwaysToLists(BuildingRoom room)
	{
		foreach (Doorway doorway in room.doorways)
		{
			availableDoorways.Add(doorway);
			allDoorways.Add(doorway);
		}
	}

	void AddDoorwayToLists(Doorway doorway)
	{
		availableDoorways.Add(doorway);
		allDoorways.Add(doorway);
	}

}
