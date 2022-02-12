using Mirror;
using System;
using System.Collections;   
using System.Collections.Generic;
using UnityEngine;

public class AsteroidNetworkManager : NetworkManager
{
    public override void OnClientError(Exception exception)
    {
        base.OnClientError(exception);
        Debug.Log("Error de conexion");
    }
}
