using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkyboxCamera : MonoBehaviour
{
    float counter = 0;

    public float rotationSpeed;
    public Vector3 rotationAxis;

    private void Update()
    {
        counter += Time.deltaTime;
        Quaternion rotationQuaternion = Quaternion.AngleAxis(counter * rotationSpeed, rotationAxis);
        transform.rotation = rotationQuaternion;
    }
}
