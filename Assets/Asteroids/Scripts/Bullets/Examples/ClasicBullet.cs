using Mirror;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ClasicBullet : BulletEntity
{
    [SerializeField] private float maxLifetime = 5f;

    [SerializeField] private BulletActor bulletActor;

    [SerializeField] private IBulletAble movementBehaviour;

    private void Awake()
    {
        if (bulletActor == null)
        {
            bulletActor = GetComponent<BulletActor>();
        }

        if (movementBehaviour == null)
        {
            movementBehaviour = GetComponentInChildren<IBulletAble>();
        }
    }

    private void FixedUpdate()
    {
        movementBehaviour?.UpdateBehaviour(Time.deltaTime);
    }

    [Server]
    public override void Init(Vector2 direction, GameObject _shooter, Vector2 position, Quaternion rotation)
    {
        CancelInvoke();

        shooter = _shooter;

        gameObject.SetActive(true);

        movementBehaviour.SetTrajectory(direction);

        ShowToClients(_shooter.transform.position, transform.rotation);

        Invoke(nameof(HideBullet), maxLifetime);
    }

    [Server]
    private void HideBullet()
    {
        bulletActor.RigidBody.RemoveVelocity();
        OnAddToPool?.Invoke(Id, gameObject);
        HideToClients();
        CancelInvoke();
    }

    [ClientRpc]
    public void HideToClients()
    {
        transform.position = Vector2.one * 100;
    }

    [ClientRpc]
    public void ShowToClients(Vector2 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HideBullet();
    }
}
