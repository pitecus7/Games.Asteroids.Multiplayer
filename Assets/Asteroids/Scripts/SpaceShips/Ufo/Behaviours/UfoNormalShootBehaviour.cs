using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoNormalShootBehaviour : NetworkBehaviour, IShootAble
{
    [SerializeField] private SpaceshipActor spaceshipActor;
    [SerializeField] private UfoSpaceship ufoEntity;

    [SerializeField] private float distanceToShoot = 10.0f;

    [SerializeField] private float fireCooldown = 1;

    private float currentTimeShoot;

    private GenericFactory factory;
    public GameObject Gameobject => gameObject;

    private void Awake()
    {
        factory = GenericFactory.Instance;

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
        if (ufoEntity.Target != null && !ufoEntity.Target.isInactive && Vector2.Distance(spaceshipActor.transform.position, ufoEntity.Target.transform.position) < distanceToShoot && currentTimeShoot >= fireCooldown)
        {
            Shoot(transform.position);
            currentTimeShoot = 0;
        }
        currentTimeShoot += dt;
    }

    public void Shoot(Vector2 position)
    {
        ClasicBullet bullet = factory.Create<ClasicBullet>("UfoBullet");

        bullet.Init(transform.up, transform.parent.gameObject, transform.parent.transform.position, transform.parent.gameObject.transform.rotation);
    }
}
