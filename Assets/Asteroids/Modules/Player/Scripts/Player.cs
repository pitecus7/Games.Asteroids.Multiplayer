using Mirror;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerEvent : UnityEvent<Player>{}

[System.Serializable]
public class IntEvent : UnityEvent<int>{}

[System.Serializable]
public class StringEvent : UnityEvent<string>{}

public class Player : NetworkBehaviour
{
    [SerializeField] private PlayersChannel playersChannel;

    [SerializeField] private SettingsChannel settinsChannel;

    public IntEvent OnLivesChange;
    public IntEvent OnScoreChange;
    public StringEvent OnNicknameChange;
    public PlayerEvent OnPlayerDead;
    public PlayerEvent OnPlayerCrash;


    [SerializeField] private PlayerNickname playerNickname;

    public int playerId = 0;

    private float respawnDelay = 3f;
    public float RespawnDelay { get => respawnDelay; }

    public int Lives { get => lives; }
    public int Score { get => score; }
    public string Nickname { get => nickname; }

    [SyncVar(hook = nameof(ScoreChanged)), SerializeField] private int score = 0;
    [SyncVar(hook = nameof(LiveChanged)), SerializeField] private int lives = 3;
    [SyncVar(hook = nameof(NicknameChanged)), SerializeField] private string nickname;

    private void Start()
    {
        playersChannel.AddPlayer(this);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        CMDInitValues(settinsChannel.nicknamePlayer);
    }

    [Command]
    public void CMDInitValues(string _nickname)
    {
        nickname = _nickname;
    }

    [ClientRpc]
    public void PlayerStart()
    {
        //TODO: Can move now
        playerNickname.HideNickname();
    }

    [ClientRpc]
    public void FinishMatch(string winnerNickname, string score)
    {
        Player player = GameplayController.Instance.Players.Find(playerEa => playerEa.Nickname == winnerNickname);
        GameplayController.Instance.GameOverPanel.ShowPanel(winnerNickname, score, player.isLocalPlayer);
    }

    private void ScoreChanged(int oldValue, int newValue)
    {
        OnScoreChange?.Invoke(newValue);
    }

    private void LiveChanged(int oldValue, int newValue)
    {
        OnLivesChange?.Invoke(newValue);
    }

    private void NicknameChanged(string oldValue, string newValue)
    {
        OnNicknameChange?.Invoke(newValue);
        playerNickname.ShowNickname(newValue);
    }
    [Server]
    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }
    [Server]
    public void RemoveScore(int scoreToRemove)
    {
        if (score - scoreToRemove >= 0)
            score -= scoreToRemove;
        else
            score = 0;
    }
    [Server]
    public void AddLive(int liveToAdd)
    {
        lives += liveToAdd;
    }
    [Server]
    public void RemoveLive(int liveToRemove)
    {
        if (lives - liveToRemove >= 0)
        {
            lives -= liveToRemove;
        }
        else
        {
            lives = 0;
        }

        if (lives == 0)
        {
            OnPlayerDead?.Invoke(this);
        }
    }

    [ClientRpc]
    public void HidePlayer()
    {
        gameObject.SetActive(false);
    }
    [ClientRpc]
    public void Respawn(Vector2 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    [ClientRpc]
    public void ShowExplosive()
    {
        GameplayController.Instance.ExplosionEffect.transform.position = transform.position;
        GameplayController.Instance.ExplosionEffect.Play();
    }

    [Server]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            ShowExplosive();
            OnPlayerCrash?.Invoke(this);
        }
    }

    private void OnDestroy()
    {
        OnPlayerDead.RemoveAllListeners();
        OnPlayerCrash.RemoveAllListeners();
        OnScoreChange.RemoveAllListeners();
        OnLivesChange.RemoveAllListeners();
    }
}
