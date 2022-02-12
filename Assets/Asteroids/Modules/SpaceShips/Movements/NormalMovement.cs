using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMovement : NetworkBehaviour, IFlyAble
{
    [SerializeField] private SpaceshipActor spaceshipActor;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float speedRotation = 5.0f;

    [SerializeField] private InputsReader inputsReader;

    private void Awake()
    {
        if (spaceshipActor == null)
        {
            spaceshipActor = GetComponentInParent<SpaceshipActor>();
        }

        if (inputsReader == null)
        {
            inputsReader = Resources.Load<InputsReader>("Common/ControlChannel");
        }        
    }

    public void UpdateBehaviour(float dt)
    {
        if (spaceshipActor == null || inputsReader == null)
        {
            Debug.LogWarning("Something wrong... Components not found.");
            return;
        }

        if (inputsReader.IsThrusting())
        {
            spaceshipActor.RigidBody.AddForce(transform.up * speed);
        }

        if (inputsReader.GetTurnDirection() != 0)
        {
            spaceshipActor.RigidBody.AddTorque(inputsReader.GetTurnDirection() * speedRotation);
        }
    }
}
