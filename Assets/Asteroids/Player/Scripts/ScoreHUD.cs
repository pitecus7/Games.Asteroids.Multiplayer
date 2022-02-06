using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHUD : MonoBehaviour
{
    [SerializeField] private PlayersChannel playersChannel;

    [SerializeField] private Text scoreText;

    void Start()
    {
        playersChannel.OnPlayerJoined += OnPlayerJoin;
    }

    private void OnPlayerJoin(Player player)
    {
        if (player.isLocalPlayer)
        {
            player.OnScoreChange.AddListener(OnScoreChanged);
        }
    }

    private void OnScoreChanged(int score)
    {
        scoreText.text = score.ToString("00000000");
    }

    private void OnDestroy()
    {
        playersChannel.OnPlayerJoined -= OnPlayerJoin;
    }
}
