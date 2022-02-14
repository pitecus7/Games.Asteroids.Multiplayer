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
        spaceshipActor.transform.up = Vector2.Lerp(spaceshipActor.transform.up, (ufoEntity.Target.position - spaceshipActor.transform.position), turnRate);

        if (Vector2.Distance(spaceshipActor.transform.position, ufoEntity.Target.position) > maxDistance)
        {
            spaceshipActor?.RigidBody.AddForce(transform.up * speed);
        }
    }
}
