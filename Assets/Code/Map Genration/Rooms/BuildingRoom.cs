using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRoom : MonoBehaviour
{
    public List<Doorway> doorways;
    public MeshCollider[] meshColliders;
    public Doorway generatedFrom;
    public SwitchableRoom switchableRoom;
    public List<GameObject> mapIcons;

    public Bounds[] RoomBounds
    {
        get
        {
            List<Bounds> bounds = new List<Bounds>();

            foreach(MeshCollider c in meshColliders)
            {
                bounds.Add(c.bounds);
            }

            return bounds.ToArray();
        }
    }
}
