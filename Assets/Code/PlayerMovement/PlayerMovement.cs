using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3;

    Transform playerTransform;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = this.transform;
    }

    void Update()
    {
        Vector3 moveDir = Vector3.zero;
        moveDir += Input.GetAxisRaw("Vertical") * playerTransform.forward;
        moveDir += Input.GetAxisRaw("Horizontal") * playerTransform.right;

        moveDir.Normalize();
        rb.velocity = moveDir * speed;
    }
}
