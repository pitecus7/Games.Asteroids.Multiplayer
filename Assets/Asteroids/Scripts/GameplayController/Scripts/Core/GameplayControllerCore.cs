using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameplayControllerCore : MonoBehaviour
{
    [SerializeField] protected List<IPlayerNetworkable> players = new List<IPlayerNetworkable>();

    [SerializeField] protected PlayersChannel playersChannel;

    [SerializeField] protected GameplayViewCore gameplayView;

    public static GameplayControllerCore Instance;

    /// <summary>
    /// Using to Init Components and to be sure that gameplay can start
    /// </summary>
    /// <param name="data">Data need to init components</param>
    /// <param name="callback">Response used to know when gameplay components are ready</param>
    public abstract void Init(object data, Action callback);

    virtual protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (playersChannel != null)
        {
            playersChannel.OnPlayerJoined += OnPlayerJoined;
        }

        if (gameplayView == null)
        {
            gameplayView = FindObjectOfType<GameplayViewCore>();
        }
    }

    protected virtual void OnPlayerJoined(IPlayerNetworkable player)
    {
        players.Add(player);
    }

    virtual protected void OnDestroy()
    {
        if (playersChannel != null)
        {
            playersChannel.OnPlayerJoined -= OnPlayerJoined;
        }
    }
}
