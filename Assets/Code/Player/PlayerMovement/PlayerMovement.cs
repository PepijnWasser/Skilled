using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        Vector2 input = InputManager.controls.Game.Movement.ReadValue<Vector2>();
        moveDir += input.y * playerTransform.forward;
        moveDir += input.x * playerTransform.right;


        moveDir.Normalize();
        moveDir.y = velocityY;

        rb.velocity = moveDir * speed * Time.deltaTime;
    }
}
