using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpaceship : SpaceshipEntity
{
    public IFlyAble spaceshipMovement;

    private void Awake()
    {
        if (spaceshipMovement == null)
        {
            spaceshipMovement = GetComponentInChildren<IFlyAble>();
        }
    }
    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        spaceshipMovement?.UpdateBehaviour(Time.deltaTime);
    }
}
