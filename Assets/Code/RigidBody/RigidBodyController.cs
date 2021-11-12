using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyController : MonoBehaviour
{
    float secondCounter = 0;
    public int updateFrequency;

    GameState gameState;
    GameObject thisGameobject;
    Rigidbody thisRigidbody;

    RigidBodyManager rigidbodyManager;
    GameManager gameManager;

    Vector3 oldPos;
    Vector3 oldRot;

    public int rigidbodyID = 0;
    public int lastPlayerTouchedID = 0;

    private void Awake()
    {
        gameState = GameObject.FindObjectOfType<GameState>();
        rigidbodyManager = GameObject.FindObjectOfType<RigidBodyManager>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        thisGameobject = this.gameObject;
        thisRigidbody = GetComponent<Rigidbody>();

        GameState.rigidbodyUpdater += MoveObject;
        GameState.rigidbodyTouchedUpdate += SetLastTouched;
    }

    private void Start()
    {
        rigidbodyID = rigidbodyManager.GetNewID();
    }

    private void OnDestroy()
    {
        GameState.rigidbodyUpdater -= MoveObject;
        GameState.rigidbodyTouchedUpdate -= SetLastTouched;
    }

    private void Update()
    {
        if (lastPlayerTouchedID == PlayerInfo.playerID)
        {
            TestSend();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == gameManager.playerCharacter)
        {
            gameState.SendLastPlayerTouchedMessage(PlayerInfo.playerID, rigidbodyID);
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
                    Debug.Log("sending poosition");
                    gameState.SendRigidBodyPosition(rigidbodyID, thisGameobject.transform.position, thisGameobject.transform.rotation.eulerAngles);
                    oldPos = thisGameobject.transform.position;
                    oldRot = thisGameobject.transform.rotation.eulerAngles;
                }
            }
        }
    }

    void MoveObject(int ID, Vector3 newPosition, Vector3 newRotation)
    {
        Debug.Log("moving object");
        if(ID == rigidbodyID)
        {
            thisGameobject.transform.position = newPosition;
            thisGameobject.transform.rotation = Quaternion.Euler(newRotation);
        }
    }

    void SetLastTouched(int _playerID, int _rigidbodyID)
    {
        if(rigidbodyID == _rigidbodyID)
        {
            lastPlayerTouchedID = _playerID;

            if (lastPlayerTouchedID == PlayerInfo.playerID)
            {
                thisRigidbody.isKinematic = false;
            }
            else
            {
                thisRigidbody.isKinematic = true;
            }
        }
    }
}
