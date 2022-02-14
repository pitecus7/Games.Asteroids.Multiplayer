using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsSpawner : NetworkBehaviour
{
    public static PowerUpsSpawner Instance;
    [SerializeField] private GenericFactory factory;

    [SerializeField] private GameDataChannel gameDataChannel;

    private void Awake()
    {
        Instance = this;

        if (factory == null)
        {
            factory = GenericFactory.Instance;
        }
    }

    public void Init()
    { }
}
