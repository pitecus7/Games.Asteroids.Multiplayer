#if MIRROR
using Mirror;
using System;
#endif
using UnityEngine;

public abstract class SpaceshipEntity :
#if MIRROR
    NetworkBehaviour
#else
    MonoBehaviour
#endif
    , ISpawneable
{
    public Action<string, GameObject> OnAddToPool { get; set; }

    [SerializeField] protected GameDataChannel gameDataChannel;

    [SerializeField] private string id;
    public string Id => id;

    public GameObject GameObject => gameObject;

    public abstract void Init();
}
