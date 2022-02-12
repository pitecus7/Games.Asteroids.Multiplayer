using Mirror;
using UnityEngine;

public class NormalShoot : NetworkBehaviour, IShootAble
{
    [SerializeField] private InputsReader inputsReader;

    private BulletsPool bulletsPool;

    private Player player;

    private void Awake()
    {
        bulletsPool = BulletsPool.Instance;

        player = GetComponentInParent<Player>();

        if (inputsReader == null)
        {
            inputsReader = Resources.Load<InputsReader>("Common/ControlChannel");
        }        
    }

    [Command]
    private void Shoot()
    {
        Bullet bullet = bulletsPool.GetPooledObject(transform.position, transform.rotation);
        bullet.Project(transform.up, player);
    }

    public void UpdateBehaviour(float dt)
    {
        if (inputsReader == null)
        {
            Debug.LogWarning("Something wrong... Components not found.");
            return;
        }
        if (inputsReader.IsShooting())
        {
            Shoot();
        }
    }
}
