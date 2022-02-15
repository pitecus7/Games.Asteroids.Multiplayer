using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloGameplayView : GameplayViewCore
{
    [SerializeField] private GameOver gameOverPanel;
    [SerializeField] private LivesHUD livesHUD;
    [SerializeField] private ScoreHUD scoreHUD;

    public override void FinishMatch(object Data = null)
    {
        PlayerSpaceship player = FindObjectOfType<PlayerSpaceship>();
        PlayerScore playerScore = player.GetComponentInChildren<PlayerScore>();

        gameOverPanel.ShowSoloPanel(playerScore.Score.ToString("0000000"));
    }

    public override void StartMatch(object Data)
    {

    }
}
