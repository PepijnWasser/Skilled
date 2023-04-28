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
	public List<SpawnableRoom> roomPrefabs = new List<SpawnableRoom>();

	public List<Doorway> allDoorways = new List<Doorway>();
	public List<Doorway> availableDoorways = new List<Doorway>();

	public EnergyUserSection sectionPower;

	public Doorway startdoor;

	public Color roomColor;

	LayerMask roomLayerMask;

	int roomsToSpawn = 0;
	int sectionID;

	public delegate void Completed(bool succeeded);
	public static event Completed OnCompletion;


    public void Initialize(LayerMask mask, int _roomsToSpawn, int _newSectionID)
	{
		roomLayerMask = mask;
		roomsToSpawn = _roomsToSpawn;
		sectionID = _newSectionID;
		AddDoorwayToLists(startdoor);
	}

	public void StartGeneration()
    {
		StartCoroutine("GenerateLevel");
	}

	IEnumerator GenerateLevel()
	{
		bool succesfullLevelGenerated = true;

		for (int i = 0; i < roomsToSpawn; i++)
		{
			yield return new WaitForFixedUpdate();
			yield return 200;
			if(SpawnRoom() == false)
            {
				succesfullLevelGenerated = false;

				break;
            }
			RemoveDoorsInSameSpace();
		}
		OnCompletion?.Invoke(succesfullLevelGenerated);
		StopCoroutine("GenerateLevel");
	}

	bool SpawnRoom()
    {
		List<Doorway> existingDoorways = availableDoorways;
		List<Doorway> existingDoorwaysChecked = new List<Doorway>();
		Doorway existingDoorwayToCheck = Extensions.RandomListItem(existingDoorways);

		while (existingDoorwaysChecked.Count < existingDoorways.Count)
        {
			existingDoorwayToCheck = Extensions.Next(existingDoorways, existingDoorwayToCheck);
			existingDoorwaysChecked.Add(existingDoorwayToCheck);
			
            if (TestRooms(existingDoorwayToCheck))
            {
				RemoveDoorsInSameSpace();
				return true;
            }
			//no room can fit on the door
            else
            {
				availableDoorways.Remove(existingDoorwayToCheck);
				existingDoorwayToCheck.GetComponent<MeshRenderer>().material.color = Color.yellow;
            }			
        }
		return false;
    }

	bool TestRooms(Doorway doorwayToFitOn)
    {
		List<SpawnableRoom> roomsToCheck = roomPrefabs;
		List<SpawnableRoom> roomsChecked = new List<SpawnableRoom>();
		SpawnableRoom roomToCheck = Extensions.RandomListItem(roomsToCheck);

		while (roomsChecked.Count < roomsToCheck.Count)
        {
			roomToCheck = Instantiate(Extensions.Next(roomsToCheck, roomToCheck), this.transform);
			roomsChecked.Add(roomToCheck);
			
			if(TestDoors(doorwayToFitOn, roomToCheck))
            {
				AddDoorwaysToLists(roomToCheck);

				foreach(GameObject mapIcon in roomToCheck.mapIcons)
                {
					mapIcon.GetComponent<Renderer>().material.color = roomColor;

				}
				roomToCheck.GetComponent<SpawnableRoom>().generatedFrom = doorwayToFitOn;
				roomToCheck.sectionId = sectionID;

				return true;
            }
            else
            {
				Destroy(roomToCheck.gameObject);
            }
			
        }
		return false;
    }

	bool TestDoors(Doorway doorwayToFitOn, SpawnableRoom buildingToTest)
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

	bool TestDoor(Doorway doorwayToFitOn, SpawnableRoom room, Doorway doorwayToTest)
	{
		PositionRoomAtDoorway(room, doorwayToTest, doorwayToFitOn);
		if (!CheckRoomOverlap(room))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	void PositionRoomAtDoorway(SpawnableRoom room, Doorway roomDoorway, Doorway targetDoorway)
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


	bool CheckRoomOverlap(SpawnableRoom room)
	{
		Bounds[] bounds = room.RoomBounds;
		for (int i = 0; i < bounds.Count(); i++)
		{
			bounds[i].center = room.transform.position;
			//bounds[i].Expand(-0.01f);
		}

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
					doorway.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
					availableDoorways.Remove(doorway);

					doorway2.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
					availableDoorways.Remove(doorway2);
				}
			}
		}
	}

	void AddDoorwaysToLists(SpawnableRoom room)
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

	public void RemoveAvailibleDoorway(Doorway d)
    {
        if (availableDoorways.Contains(d))
        {
			availableDoorways.Remove(d);
        }
    }

	public List<Doorway> GetAllDoorWaysExceptStart()
    {
		List<Doorway> _allDoorways = allDoorways;
		_allDoorways.Remove(startdoor);
		return _allDoorways;
    }

	public List<Doorway> GetAllOpenDoorways()
    {
		List<Doorway> _allOpenDoorways = availableDoorways;
		return _allOpenDoorways;
	}
}
