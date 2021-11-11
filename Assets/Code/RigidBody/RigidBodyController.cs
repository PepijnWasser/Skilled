using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyController : MonoBehaviour
{
    float secondCounter = 0;
    public int updateFrequency;

    GameState gameState;
    GameObject thisGameobject;
    RigidBodyManager manager;

    Vector3 oldPos;
    Vector3 oldRot;

    public int id = 0;

    bool needToCheck = true;

    float secondCounterReset = 0;

    private void Awake()
    {
        gameState = GameObject.FindObjectOfType<GameState>();
        manager = GameObject.FindObjectOfType<RigidBodyManager>();
        thisGameobject = this.gameObject;

        GameState.rigidbodyUpdater += MoveObject;
    }

    private void Start()
    {
        id = manager.GetNewID();
    }

    private void OnDestroy()
    {
        GameState.rigidbodyUpdater -= MoveObject;
    }

    private void Update()
    {
        if (needToCheck)
        {
            TestSend();
        }
        else
        {
            secondCounterReset += Time.deltaTime;
            if(secondCounterReset > 0.1f)
            {
                secondCounterReset = 0;

                needToCheck = true;
            }
        }
    }

    void TestSend()
    {
        secondCounter += Time.deltaTime;
        if (secondCounter >= (float)1 / (float)updateFrequency)
        {
            secondCounter = 0;
            if (thisGameobject != null)
            {
                if (thisGameobject.transform.position != oldPos || thisGameobject.transform.rotation.eulerAngles != oldRot)
                {
                    gameState.SendRigidBodyPosition(id, thisGameobject.transform.position, thisGameobject.transform.rotation.eulerAngles);
                    oldPos = thisGameobject.transform.position;
                    oldRot = thisGameobject.transform.rotation.eulerAngles;

                }
            }
        }
    }

    void MoveObject(int ID, Vector3 newPosition, Vector3 newRotation)
    {
        Debug.Log("moving object");
        if(ID == id)
        {
            thisGameobject.transform.position = newPosition;
            thisGameobject.transform.rotation = Quaternion.Euler(newRotation);

            needToCheck = false;
        }
    }
}
