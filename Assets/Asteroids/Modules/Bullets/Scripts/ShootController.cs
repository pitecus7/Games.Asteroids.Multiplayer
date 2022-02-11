using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : NetworkBehaviour 
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    [Command]
    private void Shoot()
    {
        Bullet bullet = BulletsPool.Instance.GetPooledObject(transform.position, transform.rotation);
        bullet.Project(transform.up, player);
    }
}
