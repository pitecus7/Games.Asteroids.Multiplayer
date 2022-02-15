using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesHUD : MonoBehaviour
{
    [SerializeField] private Text livesText;
    [SerializeField] private PlayersChannel playersChannel;

    void Awake()
    {
        if (playersChannel)
        {
            playersChannel.OnPlayerJoined += OnPlayerJoin;
        }
    }

    private void OnPlayerJoin(IPlayerNetworkable player)
    {
        PlayerLives lives = player.GameObject.GetComponentInChildren<PlayerLives>();

        if (lives.isLocalPlayer)
        {
            lives.OnLivesChange.AddListener(OnLivesChanged);
        }
    }

    private void OnLivesChanged(int value)
    {
        livesText.text = value.ToString();
    }

    private void OnDestroy()
    {
        if (playersChannel)
            playersChannel.OnPlayerJoined -= OnPlayerJoin;
    }
}
