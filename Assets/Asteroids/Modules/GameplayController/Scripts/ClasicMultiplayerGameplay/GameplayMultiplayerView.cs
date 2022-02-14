using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayMultiplayerView : GameplayViewCore
{
    [SerializeField] private GameOver gameOverPanel;

    [SerializeField] private Text waitPanel;

    [SerializeField] private LivesHUD livesHUD;
    [SerializeField] private ScoreHUD scoreHUD;

    public override void FinishMatch(object Data)
    {

    }

    public override void StartMatch(object Data)
    {
        waitPanel.gameObject.SetActive(false);
    }
}
