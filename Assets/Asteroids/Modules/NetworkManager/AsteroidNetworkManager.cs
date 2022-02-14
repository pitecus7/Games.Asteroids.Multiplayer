using kcp2k;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidNetworkManager : NetworkManager
{
    [SerializeField] private ProjectSettingsSO projectSettings;
    public override void Awake()
    {
        projectSettings.projectObjects.ForEach(spawnObject =>
        {
            if (!spawnPrefabs.Contains(spawnObject.prefab))
            {
                spawnPrefabs.Add(spawnObject.prefab);
            }
        });
        base.Awake();
    }

    public void Init(bool solo)
    {
        if (solo)
        {
            GetComponent<KcpTransport>().Port = 69;
        }
    }

    public override void OnClientError(Exception exception)
    {
        base.OnClientError(exception);
        Debug.Log("Error de conexion");
    }
}
