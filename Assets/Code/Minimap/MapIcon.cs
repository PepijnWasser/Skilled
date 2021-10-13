using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIcon : MonoBehaviour
{
    public Vector3 targetPos;

    public GameObject parent;

    private void Start()
    {
        this.transform.rotation = Quaternion.identity; 
    }

    private void Update()
    {
        this.transform.position = new Vector3(parent.transform.position.x, targetPos.y, parent.transform.position.z);
    }
}
