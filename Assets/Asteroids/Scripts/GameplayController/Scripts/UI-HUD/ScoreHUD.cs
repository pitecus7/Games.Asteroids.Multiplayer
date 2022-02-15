using UnityEngine;
using UnityEngine.UI;

public class ScoreHUD : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private PlayersChannel playersChannel;

    void Start()
    {
        if (playersChannel)
            playersChannel.OnPlayerJoined += OnPlayerJoin;
    }

    private void OnPlayerJoin(IPlayerNetworkable player)
    {
        PlayerScore score = player.GameObject.GetComponentInChildren<PlayerScore>();
        if (score.isLocalPlayer)
        {
            score.OnScoreChange.AddListener(OnScoreChanged);
        }
    }

    private void OnScoreChanged(int score)
    {
        scoreText.text = score.ToString("00000000");
    }

    private void OnDestroy()
    {
        if (playersChannel)
            playersChannel.OnPlayerJoined -= OnPlayerJoin;
    }
}
