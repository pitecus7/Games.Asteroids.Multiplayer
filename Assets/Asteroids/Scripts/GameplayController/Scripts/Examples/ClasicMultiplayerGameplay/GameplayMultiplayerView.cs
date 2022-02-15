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
        Debug.Log("finally");
    }

    public void FinishMatch()
    {
        List<PlayerScore> scores = new List<PlayerScore>();
        string winnerNickname = "";
        int scoreMax = 0;
        bool isWinner = false;

        players.ForEach(player =>
        {
            PlayerScore playerScore = player.GameObject.GetComponentInChildren<PlayerScore>();
            if (playerScore != null && playerScore.Score >= scoreMax)
            {
                scoreMax = playerScore.Score;
                winnerNickname = player.Nickname;
                isWinner = player.isLocalPlayer;
            }
            gameOverPanel.ShowPanel(winnerNickname, scoreMax.ToString("0000000"), isWinner);

        });
    }

    public override void StartMatch(object Data)
    {
        waitPanel.gameObject.SetActive(false);
        players.AddRange(FindObjectsOfType<PlayerSpaceship>());
    }

    private void OnDestroy()
    {
        if (gameDataChannel != null)
            gameDataChannel.OnFinishMatch -= FinishMatch;
    }
}
