using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemLocation : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, transform.rotation * -Vector3.forward);
        Gizmos.color = Color.white;
        Gizmos.DrawRay(ray);
    }
}
