using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : NetworkBehaviour, ISpawneable
{
    public abstract void Init(Vector2 position);

    public Action<string, GameObject> OnAddToPool { get; set; }

    public abstract string Id { get; }

    public GameObject GameObject => gameObject;
}
