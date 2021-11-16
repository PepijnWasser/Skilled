using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorTest : MonoBehaviour
{
    public GameObject obj;

    private void Start()
    {
        Quaternion q = transform.rotation * transform.rotation;
        Matrix4x4 m = Matrix4x4.Rotate(q);

        obj.transform.position = m * obj.transform.position;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.up * 3, Color.green);
        Debug.DrawRay(transform.position, transform.right * 3, Color.green);
        Vector3 transformVector = transform.right;
        Vector3 test = Quaternion.Euler(transform.rotation * transformVector).eulerAngles;
        Debug.DrawRay(transform.position, test * 3, Color.red);

        transform.rotation = Quaternion.Euler(test);
    }
}
