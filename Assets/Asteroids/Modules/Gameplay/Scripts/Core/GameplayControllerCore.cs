using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameplayControllerCore<T> : MonoBehaviour where T : Object
{
    public static T Instance;
    virtual protected void Awake()
    {
        Instance = FindObjectOfType<T>();
    }
}
