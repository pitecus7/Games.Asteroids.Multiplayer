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
            if (spawnObject != null && spawnObject.prefab)
            {
                if (!spawnPrefabs.Contains(spawnObject.prefab))
                {
                    spawnPrefabs.Add(spawnObject.prefab);
                }
            }
        });
        base.Awake();
    }

    public void Init(bool solo, string _address, ushort port)
    {
        if (solo)
        {
            GetComponent<KcpTransport>().Port = 69;
        }
        else
        {
            if (!string.IsNullOrEmpty(_address))
            {
                networkAddress = _address;
            }
            else
            {
                networkAddress = "localhost";
            }
            if (port > 0)
            {
                GetComponent<KcpTransport>().Port = port;
            }
            else
            {
                GetComponent<KcpTransport>().Port = 7777;
            }
        }
    }

    public override void OnClientError(Exception exception)
    {
        base.OnClientError(exception);
        Debug.Log("Error de conexion");
    }
}
