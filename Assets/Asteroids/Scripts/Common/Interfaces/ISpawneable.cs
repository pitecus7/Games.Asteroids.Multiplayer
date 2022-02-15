using System;
using UnityEngine;

public interface ISpawneable
{
    public Action<string,GameObject> OnAddToPool { get; set; }
    public string Id { get; }

    public GameObject GameObject { get; }
}
