using System;
using UnityEngine;

public class RotateSpaceObject : MonoBehaviour
{
    float counter = 0;

    public float spaceRotationSpeed;
    public Vector3 spaceRotationAxis;

    public float localRotationSpeed;
    public Vector3 localRotationAxis;

    private void Update()
    {
        counter += Time.deltaTime;

        Debug.DrawRay(Vector3.zero, spaceRotationAxis * 20, Color.red);
        Debug.DrawRay(transform.position, localRotationAxis * 20, Color.cyan);

        transform.RotateAround(Vector3.zero, spaceRotationAxis, -spaceRotationSpeed * Time.deltaTime);
        transform.LookAt(Vector3.zero);

        transform.rotation *= Quaternion.AngleAxis(counter * localRotationSpeed, localRotationAxis);

    }
}