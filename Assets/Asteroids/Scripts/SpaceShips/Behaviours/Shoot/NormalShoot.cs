using Mirror;
using UnityEngine;

public class NormalShoot : NetworkBehaviour, IShootAble
{
    [SerializeField] private float fireCooldown = 1;

    private float currentTimeShoot;

    private GenericFactory factory;
    public GameObject Gameobject => gameObject;

    private void Awake()
    {
        factory = GenericFactory.Instance;
    }

    public void Shoot(Vector2 position)
    {
        if (currentTimeShoot >= fireCooldown)
        {
            currentTimeShoot = 0;
            ClasicBullet bullet = factory.Create<ClasicBullet>("ClasicBullet");
            bullet.Init(transform.up, transform.parent.gameObject, position, transform.parent.gameObject.transform.rotation);
        }
    }

    public void UpdateBehaviour(float dt)
    {
        currentTimeShoot += dt;
    }
}
