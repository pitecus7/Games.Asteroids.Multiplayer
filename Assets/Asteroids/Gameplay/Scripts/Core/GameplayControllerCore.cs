using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameplayControllerCore<T> : MonoBehaviour where T : UnityEngine.Object
{
    public static T Instance;
    virtual protected void Awake()
    {
        Instance = FindObjectOfType<T>();
    }
}
