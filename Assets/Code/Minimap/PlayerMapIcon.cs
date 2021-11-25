using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapIcon : MapIcon
{
    public GameObject rotationTarget;
    public GameObject looktarget;

    protected override void Update()
    {
        base.Update();

        Vector3 targetRotation = new Vector3(0, rotationTarget.transform.rotation.eulerAngles.y + looktarget.transform.localRotation.eulerAngles.y, 0);
        this.transform.rotation = Quaternion.Euler(targetRotation);
        Debug.Log("player icon");
    }
}
