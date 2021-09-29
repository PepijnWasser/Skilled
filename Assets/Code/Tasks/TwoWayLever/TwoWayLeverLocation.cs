using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayLeverLocation : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, transform.rotation * Vector3.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);
    }
}
