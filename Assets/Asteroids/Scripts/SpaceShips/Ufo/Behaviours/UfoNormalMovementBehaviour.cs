using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class UfoNormalMovementBehaviour : NetworkBehaviour, IFlyAble
{
    [SerializeField] private SpaceshipActor spaceshipActor;
    [SerializeField] private UfoSpaceship ufoEntity;
    [SerializeField] private float speed = 1.0f;

    [SerializeField] private float maxDistance = 5.0f;
    [SerializeField] private float turnRate = 5.0f;

    public float Speed => speed;

    public float SpeedRotation => 0;

    public GameObject Gameobject => gameObject;

    private void Awake()
    {
        if (ufoEntity == null)
        {
            ufoEntity = GetComponentInParent<UfoSpaceship>();
        }

        if (spaceshipActor == null)
        {
            spaceshipActor = GetComponentInParent<SpaceshipActor>();
        }
    }

    public void UpdateBehaviour(float dt)
    {
        if (ufoEntity.Target != null && !ufoEntity.Target.isInactive)
        {
            spaceshipActor.transform.up = Vector2.Lerp(spaceshipActor.transform.up, (ufoEntity.Target.transform.position - spaceshipActor.transform.position), turnRate);

            if (Vector2.Distance(spaceshipActor.transform.position, ufoEntity.Target.transform.position) > maxDistance)
            {
                spaceshipActor?.RigidBody.AddForce(transform.up * speed);
            }
        }
        else
        {
            spaceshipActor?.RigidBody.AddForce(transform.up * speed * 2);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetSpeedRotation(float newSpeed)
    {
    }
}
