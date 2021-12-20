using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyController : MonoBehaviour
{
    [SerializeField]
    private int updateFrequency = 10;

    public int rbID = 0;
    private int lastPlayerTouchedID = 1;

    private GameState gameState;
    private Rigidbody rb;
    private RigidbodyManager rbManager;

    private float secondCounter = 0;
    private float updateTime;

    private Vector3 oldPos;
    private Vector3 oldRot;

    private void Awake()
    {
        gameState = GameObject.FindObjectOfType<GameState>();
        rbManager = GameObject.FindObjectOfType<RigidbodyManager>();
        rb = GetComponent<Rigidbody>();

        GameState.rigidbodyUpdater += MoveObject;
        GameState.rigidbodyTouchedUpdate += SetLastTouched;

        updateTime = (float)1 / (float)updateFrequency;
    }

    //when we are created we assign a new ID from the rigidbody Manager
    private void Start()
    {
        if(rbManager != null)
        {
            rbID = rbManager.GetNewID();
        }
    }

    //unsubsribe from events when we are destroyed
    private void OnDestroy()
    {
        GameState.rigidbodyUpdater -= MoveObject;
        GameState.rigidbodyTouchedUpdate -= SetLastTouched;
    }

    //If we are the player that last touched this rigidbody, test if we need to send a new position to the server
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
            gameState.SendLastPlayerTouchedMessage(PlayerInfo.playerID, rbID);
        }
        else if(collision.collider.gameObject.GetComponent<RigidBodyController>() != null)
        {
            RigidBodyController toucher = collision.collider.gameObject.GetComponent<RigidBodyController>();

            if(toucher.rbID == PlayerInfo.playerID)
            {
                if(gameState != null)
                {
                    gameState.SendLastPlayerTouchedMessage(PlayerInfo.playerID, rbID);
                }
            }
        }
    }

    //every x milliseconds we test if the transform has changed
    //if it changed, we send the new position/rotation to the server
    void TestSend()
    {
        secondCounter += Time.deltaTime;
        if (secondCounter >= updateTime)
        {
            secondCounter = 0;
            if (this.transform.position != oldPos || this.transform.rotation.eulerAngles != oldRot)
            {
                if (gameState != null)
                {
                    Debug.Log("sending position");
                    gameState.SendRigidBodyPosition(rbID, this.transform.position, this.transform.rotation.eulerAngles);
                    oldPos = this.transform.position;
                    oldRot = this.transform.rotation.eulerAngles;
                }

            }
        }
    }

    //If we get a new position from the server, we change the position the the one we got
    void MoveObject(int ID, Vector3 newPosition, Vector3 newRotation)
    {
        Debug.Log("moving object");
        if(ID == rbID)
        {
            this.transform.position = newPosition;
            this.transform.rotation = Quaternion.Euler(newRotation);
        }
    }

    //if someone else touched this object, the server will send that update to all clients
    //if we receive one of those messages, we update the lastPlayerTouchedID
    void SetLastTouched(int _playerID, int _rigidbodyID)
    {
        if(rbID == _rigidbodyID)
        {
            lastPlayerTouchedID = _playerID;

            if (lastPlayerTouchedID == PlayerInfo.playerID)
            {
                rb.isKinematic = false;
            }
            else
            {
                rb.isKinematic = true;
            }
        }
    }
}
