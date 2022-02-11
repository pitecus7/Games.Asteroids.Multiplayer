using kcp2k;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController : GameplayControllerCore<GameplayController>
{
    [SerializeField] private PlayersChannel playersChannel;

    private List<Player> players = new List<Player>();
    public List<Player> Players { get => players; }

    [SerializeField] private SettingsChannel settingChannel;

    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] public ParticleSystem ExplosionEffect => explosionEffect;

    [SerializeField] private List<Transform> spawnPositions;

    [SerializeField] private GameOver gameOverPanel;
    [SerializeField] public GameOver GameOverPanel { get => gameOverPanel; }

    [SerializeField] private Text waitPanel;

    [SerializeField] private AsteroidSpawner asteroidSpawner;

    private int playerDead = 0;

    private void Start()
    {
        playersChannel.OnPlayerJoined += OnPlayerJoined;

        if (settingChannel.gameType == GameType.Solo)
        {
            waitPanel.gameObject.SetActive(false);

            NetworkManager.singleton.gameObject.GetComponent<KcpTransport>().Port = 69;
        }

        StartConnection();
    }

    private bool firstButtonPressed;
    private float timeOfFirstButton = 0.8f;

    private bool reset;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && firstButtonPressed)
        {
            if (Time.time - timeOfFirstButton < 0.5f)
            {
                BackToLobby();
            }

            reset = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !firstButtonPressed)
        {
            firstButtonPressed = true;
            timeOfFirstButton = Time.time;
        }

        if (reset)
        {
            firstButtonPressed = false;
            reset = false;
        }
    }

    public void BackToLobby()
    {
        NetworkManager.singleton.StopHost();
        SceneManager.LoadScene("Lobby");
    }

    private void StartConnection()
    {
        if (settingChannel.gameType == GameType.Solo || (settingChannel.gameType != GameType.Solo && settingChannel.isHost))
        {
            NetworkManager.singleton.StartHost();
        }
        else
        {
            NetworkManager.singleton.StartClient();
            NetworkManager.singleton.OnClientDisconnectInternalEvent += OnClienctDisconnect;
        }
    }

    private void OnClienctDisconnect()
    {
        waitPanel.text = "<color=red>Sorry, Error on Connection, Please back to Lobby...</color>";
    }

    private void OnPlayerJoined(Player player)
    {
        players.Add(player);
        player.playerId = players.Count;
        player.OnPlayerCrash.AddListener(OnPlayerCrash);

        if (player.playerId - 1 < spawnPositions.Count && settingChannel.gameType != GameType.Solo)
        {
            player.gameObject.transform.position = spawnPositions[player.playerId - 1].position;
            player.gameObject.transform.rotation = spawnPositions[player.playerId - 1].rotation;
        }

        if (players.Count >= 2)
        {
            Invoke(nameof(StartMatch), 2);
        }
        else if (settingChannel.gameType == GameType.Solo)
        {
            asteroidSpawner.Init();
            player.PlayerStart();
        }
    }

    private void StartMatch()
    {
        waitPanel.gameObject.SetActive(false);
        asteroidSpawner.Init();

        players.ForEach(player => player.PlayerStart());
    }

    private void OnPlayerCrash(Player player)
    {
        explosionEffect.transform.position = player.transform.position;
        explosionEffect.Play();

        player.RemoveLive(1);

        if (player.Lives <= 0)
        {
            if (settingChannel.gameType == GameType.Solo)
            {
                gameOverPanel.ShowSoloPanel(player.Score.ToString("0000000"));
            }
            else
            {
                playerDead++;

                if (playerDead >= players.Count)
                {
                    string winnerNickname = "";
                    int scoreMax = 0;
                    players.ForEach(player =>
                    {
                        if (player.Score >= scoreMax)
                        {
                            scoreMax = player.Score;
                            winnerNickname = player.Nickname;

                        }
                    });
                    players.ForEach(player => player.FinishMatch(winnerNickname, scoreMax.ToString("0000000")));
                }
            }
        }
        else
        {
            StartCoroutine(Respawn(player));
        }

        player.HidePlayer();
    }

    private IEnumerator Respawn(Player player)
    {

        yield return new WaitForSeconds(player.RespawnDelay);

        if (player.playerId - 1 < spawnPositions.Count && settingChannel.gameType != GameType.Solo)
        {
            player.gameObject.transform.position = spawnPositions[player.playerId - 1].position;
            player.gameObject.transform.rotation = spawnPositions[player.playerId - 1].rotation;
        }
        else
        {
            player.gameObject.transform.position = Vector3.zero;
        }

        player.Respawn(player.transform.position);
    }

    public void AsteroidDestroyed(Asteroid asteroid, Player player)
    {
        explosionEffect.transform.position = asteroid.transform.position;
        explosionEffect.Play();

        if (asteroid.Size < 0.7f)
        {
            player.AddScore(100); // small asteroid
        }
        else if (asteroid.Size < 1.4f)
        {
            player.AddScore(50); // medium asteroid
        }
        else
        {
            player.AddScore(25); // large asteroid
        }
    }

    private void OnDestroy()
    {
        players.Clear();
        playersChannel.OnPlayerJoined -= OnPlayerJoined;
    }
}
