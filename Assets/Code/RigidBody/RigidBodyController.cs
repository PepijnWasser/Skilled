using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyController : MonoBehaviour
{
    float secondCounter = 0;
    public int updateFrequency = 10;

    GameState gameState;
    Rigidbody thisRigidbody;

    RigidBodyManager rigidbodyManager;

    Vector3 oldPos;
    Vector3 oldRot;

    public int rigidbodyID = 0;
    int lastPlayerTouchedID = 1;

    private void Awake()
    {
        gameState = GameObject.FindObjectOfType<GameState>();
        rigidbodyManager = GameObject.FindObjectOfType<RigidBodyManager>();
        thisRigidbody = GetComponent<Rigidbody>();

        GameState.rigidbodyUpdater += MoveObject;
        GameState.rigidbodyTouchedUpdate += SetLastTouched;
    }

    private void Start()
    {
        if(rigidbodyManager != null)
        {
            rigidbodyID = rigidbodyManager.GetNewID();
        }
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
        if(collision.gameObject == GameManager.playerCharacter)
        {
            gameState.SendLastPlayerTouchedMessage(PlayerInfo.playerID, rigidbodyID);
        }
        else if(collision.collider.gameObject.GetComponent<RigidBodyController>() != null)
        {
            RigidBodyController toucher = collision.collider.gameObject.GetComponent<RigidBodyController>();

            if(toucher.rigidbodyID == PlayerInfo.playerID)
            {
                if(gameState != null)
                {
                    gameState.SendLastPlayerTouchedMessage(PlayerInfo.playerID, rigidbodyID);
                }
            }
        }
    }

    void TestSend()
    {
        secondCounter += Time.deltaTime;
        if (secondCounter >= (float)1 / (float)updateFrequency)
        {
            secondCounter = 0;
            if (this.transform.position != oldPos || this.transform.rotation.eulerAngles != oldRot)
            {
                if (gameState != null)
                {
                    Debug.Log("sending position");
                    gameState.SendRigidBodyPosition(rigidbodyID, this.transform.position, this.transform.rotation.eulerAngles);
                    oldPos = this.transform.position;
                    oldRot = this.transform.rotation.eulerAngles;
                }

            }
        }
    }

    void MoveObject(int ID, Vector3 newPosition, Vector3 newRotation)
    {
        Debug.Log("moving object");
        if(ID == rigidbodyID)
        {
            this.transform.position = newPosition;
            this.transform.rotation = Quaternion.Euler(newRotation);
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
