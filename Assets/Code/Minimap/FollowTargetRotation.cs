using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetRotation : MonoBehaviour
{
    public List<GameObject> targets;
    public List<GameObject> localTargets;

    private void Update()
    {
        Debug.Log(this.transform.rotation.eulerAngles.x);
        Vector3 targetRotation = new Vector3(90, 0, 0);
        foreach (GameObject target in targets)
        {
            targetRotation.y += target.transform.rotation.eulerAngles.y;
        }
        foreach(GameObject target in localTargets)
        {
            targetRotation.y += target.transform.localRotation.eulerAngles.y;
        }


        this.transform.rotation = Quaternion.Euler(targetRotation);
    }
}
