using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMovement : MonoBehaviour, IFlyAble
{
    [SerializeField] private SpaceshipActor spaceshipActor;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float speedRotation = 5.0f;

    [SerializeField] private IControlable controlChannel;

    private void Awake()
    {
        if (spaceshipActor == null)
        {
            spaceshipActor = GetComponentInParent<SpaceshipActor>();
        }

        if (controlChannel == null)
        {
            controlChannel = Resources.Load<ControlChannel>("Common/ControlChannel");
        }
    }

    public void UpdateBehaviour(float dt)
    {
        if (spaceshipActor == null || controlChannel == null)
        {
            Debug.LogWarning("Something wrong... Components not found.");
            return;
        }

        if (controlChannel.IsThrusting())
        {
            spaceshipActor.RigidBody.AddForce(transform.up * speed);
        }

        if (controlChannel.GetTurnDirection() != 0)
        {
            spaceshipActor.RigidBody.AddTorque(controlChannel.GetTurnDirection() * speedRotation);
        }
    }
}
