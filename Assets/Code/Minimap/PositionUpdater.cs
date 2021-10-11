using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionUpdater : MonoBehaviour
{
    float secondCounter = 0;
    public int updateFrequency;
    protected MapCamera mapCamera;
    protected Vector3 oldPos;

    public delegate void UpdatePosition(Vector3 newPos, PositionUpdater updater);
    public static event UpdatePosition positionChanged;

    protected virtual void Start()
    {
        mapCamera = GetComponent<MapCamera>();
    }

    //sends the new player position/rotation through udp
    void Update()
    {
        secondCounter += Time.deltaTime;
        if (secondCounter >= (float)1 / (float)updateFrequency)
        {
            secondCounter = 0;
            if (mapCamera != null)
            {
                if (mapCamera.transform.position != oldPos)
                {
                    positionChanged?.Invoke(mapCamera.transform.position, this);
                    oldPos = mapCamera.transform.position;
                }
            }
            else
            {
                Debug.Log("no camera");
            }
        }
    }
}
