using Mirror;
using UnityEngine;

public class NormalShoot : NetworkBehaviour, IShootAble
{
    [SerializeField] private InputsReader inputsReader;

    [SerializeField] private float fireCooldown = 1;

    private float currentTimeShoot;

    private GenericFactory factory;

    public GameObject Gameobject => gameObject;

    private void Awake()
    {
        factory = GenericFactory.Instance;

        if (inputsReader == null)
        {
            inputsReader = Resources.Load<InputsReader>("Common/ControlChannel");
        }
    }

    [Command]
    private void Shoot(Vector2 position)
    {
        ClasicBullet bullet = factory.Create<ClasicBullet>("ClasicBullet");

        bullet.Init(transform.up, transform.parent.gameObject, position);
    }

    public void UpdateBehaviour(float dt)
    {
        if (inputsReader == null)
        {
            Debug.LogWarning("Something wrong... Components not found.");
            return;
        }
        if (inputsReader.IsShooting() && currentTimeShoot >= fireCooldown)
        {
            Shoot(transform.parent.transform.position);
            currentTimeShoot = 0;
        }
        currentTimeShoot += dt;
    }
}
