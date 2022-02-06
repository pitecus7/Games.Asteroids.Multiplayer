using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesHUD : MonoBehaviour
{
    [SerializeField] private PlayersChannel playersChannel;

    [SerializeField] private Text livesText;

    // Start is called before the first frame update
    void Start()
    {
        playersChannel.OnPlayerJoined += OnPlayerJoin;
    }

    private void OnPlayerJoin(Player player)
    {
        if (player.isLocalPlayer)
        {
            player.OnLivesChange.AddListener(OnLivesChanged);
        }
    }

    private void OnLivesChanged(int value)
    {
        livesText.text = value.ToString();
    }

    private void OnDestroy()
    {
        playersChannel.OnPlayerJoined -= OnPlayerJoin;
    }
}
