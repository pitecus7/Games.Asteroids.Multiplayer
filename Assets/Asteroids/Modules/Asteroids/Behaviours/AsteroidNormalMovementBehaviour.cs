using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidNormalMovementBehaviour : NetworkBehaviour, IMoveable
{
    public AsteroidActor asteroidActor;

    [SerializeField] private float movementSpeed = 50f;

    private void Awake()
    {
        if (asteroidActor == null)
        {
            asteroidActor = GetComponentInParent<AsteroidActor>();
        }
    }

    public void SetTrajectory(Vector2 direction)
    {
        float randomSpeed = Random.Range(movementSpeed - movementSpeed / 2, movementSpeed + movementSpeed / 2);
        asteroidActor?.RigidBody.AddForce(direction * randomSpeed);
    }
}
