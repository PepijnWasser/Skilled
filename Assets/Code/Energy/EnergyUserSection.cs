using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUserSection : EnergyUser
{
    public List<SwitchableRoom> switchableRooms = new List<SwitchableRoom>();

    private void Awake()
    {
        MapGenerator.onCompletion += SetIconPos;
    }

    private void OnDestroy()
    {
        MapGenerator.onCompletion -= SetIconPos;
    }

    protected override void TurnOn()
    {
        base.TurnOn();
        foreach (SwitchableRoom room in switchableRooms)
        {
            room.TurnOn();
        }
    }

    protected override void TurnOff()
    {
        base.TurnOff();
        foreach(SwitchableRoom room in switchableRooms)
        {
            room.TurnOff();
        }
    }

    public void AddRoom(SwitchableRoom room)
    {
        switchableRooms.Add(room);

        room.TurnOff();
        
    }

    void SetIconPos()
    {
        Vector3 averagePos = Vector3.zero;
        Transform[] childTransforms = transform.parent.GetComponentsInChildren<Transform>();
        List<GameObject> children = new List<GameObject>();

        foreach(Transform t in childTransforms)
        {
            children.Add(t.gameObject);
        }

        foreach (GameObject c in children)
        {
            averagePos += c.transform.position;
        }
        averagePos = averagePos / children.Count;

        this.transform.position = averagePos;
    }
}
