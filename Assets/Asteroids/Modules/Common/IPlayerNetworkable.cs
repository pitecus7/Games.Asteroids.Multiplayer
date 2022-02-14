using System;
using UnityEngine;

public interface IPlayerNetworkable
{
    public Action<IPlayerNetworkable, GameObject> OnColision { get; set; }

    public bool IsLocal { get; }

    public int PlayerId { get; }

    public string Nickname { get; }

    public GameObject GameObject { get; }

    public void Init();

    void Respawn(Vector2 position);
}
