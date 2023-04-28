using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class SpawnableRoom : MonoBehaviour
{
    public List<Doorway> doorways;
    public MeshCollider[] meshColliders;
    [ReadOnly]
    public Doorway generatedFrom;
    [ReadOnly]
    public int sectionId;
    [ReadOnly]
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
