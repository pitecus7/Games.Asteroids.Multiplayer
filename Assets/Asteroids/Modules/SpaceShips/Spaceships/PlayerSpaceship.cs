using UnityEngine;

public class PlayerSpaceship : SpaceshipEntity
{
    [SerializeField, SerializeReference]
    private IFlyAble spaceshipMovement;
    [SerializeField, SerializeReference]
    private IShootAble spaceshipShoot;

    private void Awake()
    {
        if (spaceshipMovement == null)
        {
            spaceshipMovement = GetComponentInChildren<IFlyAble>();
        }
        if (spaceshipShoot == null)
        {
            spaceshipShoot = GetComponentInChildren<IShootAble>();
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
        spaceshipShoot?.UpdateBehaviour(Time.deltaTime);
    }
}
