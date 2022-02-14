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

    [SerializeField] private GameDataChannel gameDataChannel;

    public List<PlayerSpaceship> players = new List<PlayerSpaceship>();
    private void Awake()
    {
        if (gameDataChannel != null)
            gameDataChannel.OnFinishMatch += FinishMatch;
    }

    public override void FinishMatch(object Data = null)
    {

    }

    public void FinishMatch()
    {
        Debug.Log("Finish");
    }

    public override void StartMatch(object Data)
    {
        waitPanel.gameObject.SetActive(false);
        players.AddRange(FindObjectsOfType<PlayerSpaceship>());

        Debug.Log(players.Count);
    }

    private void OnDestroy()
    {
        if (gameDataChannel != null)
            gameDataChannel.OnFinishMatch -= FinishMatch;
    }
}
