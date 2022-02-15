using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletActor : MonoBehaviour
{
    public Action<GameObject> OnColision;
    public Action<GameObject> OnTrigger;

    [SerializeField] private Rigidbody2D rigidBody;


    [SerializeField] private float mass = 0.5f;

    public Rigidbody2D RigidBody { get => rigidBody; }

    private void Awake()
    {
        if (rigidBody == null)
        {
            if (!TryGetComponent(out rigidBody))
            {
                return;
            }
        }

        rigidBody.mass = mass;

        rigidBody.gravityScale = 0;
        rigidBody.drag = 0;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnColision?.Invoke(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTrigger?.Invoke(collision.gameObject);
    }
}
