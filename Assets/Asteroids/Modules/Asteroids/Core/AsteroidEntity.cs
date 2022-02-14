using Mirror;
using System;
using UnityEngine;

public abstract class AsteroidEntity : NetworkBehaviour, ISpawneable
{
    [SerializeField] private string id;

    [SerializeField] private GameDataChannel gameDataChannel;
    public string Id => id;

    public GameObject GameObject => gameObject;

    public Action<string, GameObject> OnAddToPool { get; set; }

    public abstract void Init(object data);

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        SpaceshipEntity shooter;
        BulletEntity bullet;
        if (collision.gameObject.TryGetComponent(out bullet) && bullet.Shooter.TryGetComponent(out shooter))
            gameDataChannel.AsteroidDestroy(this, shooter);
        else if (collision.gameObject.TryGetComponent(out shooter))
            gameDataChannel.AsteroidDestroy(this, shooter);

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
