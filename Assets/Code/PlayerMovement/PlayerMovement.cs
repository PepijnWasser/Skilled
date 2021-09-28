using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3;

    Transform playerTransform;
    Rigidbody rb;

    public float gravity = 9.81f;
    float velocityY = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = this.transform;
    }

    void Update()
    {
        if(Physics.Raycast(transform.position, -transform.up, 2))
        {
            velocityY = 0;
        }
        else
        {
            velocityY -= gravity * Time.deltaTime;
        }

        Vector3 moveDir = Vector3.zero;
        moveDir += Input.GetAxisRaw("Vertical") * playerTransform.forward;
        moveDir += Input.GetAxisRaw("Horizontal") * playerTransform.right;


        moveDir.Normalize();
        moveDir.y = velocityY;

        Debug.Log(Input.GetAxisRaw("Vertical"));

        rb.velocity = moveDir * speed;
    }
}
