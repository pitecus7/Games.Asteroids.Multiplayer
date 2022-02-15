using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellPowerUp : PowerUp
{
    [SerializeField] private string id;
    public override string Id => id;

    [SerializeField] private PowerUpSO powerUpData;

    private PlayerSpaceship target;

    private IShootAble defaultShootBehaviour;

    [Server]
    public override void Init(Vector2 position)
    {
        transform.position = position;
        ShowToClients(position);
    }

    [Server]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out target))
        {
            defaultShootBehaviour = target.SpaceshipShoot;
            ArcShoot newBehaviour = target.gameObject.GetComponentInChildren<ArcShoot>();
            newBehaviour?.SetShootValues(30, 150, powerUpData.increse);
            target?.SetSpaceshipShoot(newBehaviour);
        }

        //Networking
        HideToClients();
        Invoke(nameof(RestoreTarget), powerUpData.duration);
        gameObject.transform.position = Vector2.one * 100;
    }

    [ClientRpc]
    public void SetValuesToClients()
    {
        transform.position = Vector2.one * 100;
    }

    [Server]
    private void RestoreTarget()
    {
        target?.SetSpaceshipShoot(defaultShootBehaviour);
        OnAddToPool?.Invoke(id, gameObject);
    }

    [ClientRpc]
    public void HideToClients()
    {
        transform.position = Vector2.one * 100;
    }

    [ClientRpc]
    public void ShowToClients(Vector2 position)
    {
        transform.position = position;
    }
}
