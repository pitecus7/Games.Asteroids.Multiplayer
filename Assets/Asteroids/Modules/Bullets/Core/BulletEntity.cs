using Mirror;
using System;
using UnityEngine;

public abstract class BulletEntity : NetworkBehaviour, ISpawneable
{
    public Action<string, GameObject> OnAddToPool { get; set; }

    [SerializeField] protected GameObject shooter;
    
    public GameObject Shooter => shooter;

    [SerializeField] private string id;
    public string Id => id;

    public GameObject GameObject => gameObject;
}
