using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBulletMovementBehaviour : MonoBehaviour, IMoveable
{
    [SerializeField] private BulletActor bulletActor;
    [SerializeField] private float speed = 500f;

    private void Awake()
    {
        if (bulletActor == null)
        {
            bulletActor = GetComponentInParent<BulletActor>();
        }
    }

    public void SetTrajectory(Vector2 direction)
    {
        bulletActor?.RigidBody.RemoveVelocity();
        bulletActor?.RigidBody.AddForce(direction * speed);
    }
}
