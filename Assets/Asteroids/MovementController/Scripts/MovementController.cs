using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : NetworkBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float speedRotation = 1.0f;
    [SerializeField] private Rigidbody2D rigidBody;
    private bool thrusting;
    private float turnDirection;

    void Update()
    {
        if (!isLocalPlayer)
            return;
        thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1.0f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1.0f;
        }
        else
        {
            turnDirection = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;
        if (thrusting)
        {
            rigidBody.AddForce(transform.up * speed);
        }

        if (turnDirection != 0)
        {
            rigidBody.AddTorque(turnDirection * speedRotation);
        }
    }
}
