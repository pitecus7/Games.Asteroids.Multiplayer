using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicMultiplayerGameplayController : GameplayControllerCore
{
    [SerializeField] private List<Transform> spawnPositions;

    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private EnemySpawner enemySpawner;

    [SerializeField] private GameDataChannel gameDataChannel;

    [SerializeField] private ProjectSettingsSO projectSettings;

    [SerializeField] private uint maxPlayers = 2;

    [SerializeField] private float respawnDelay = 3f;

    private int deadPlayersCount = 0;

    public override void Init(object data, Action callback)
    {
        if (!asteroidSpawner)
        {
            asteroidSpawner = AsteroidSpawner.Instance;
        }
        if (!enemySpawner)
        {
            enemySpawner = EnemySpawner.Instance;
        }

        callback?.Invoke();
    }

    protected override void OnPlayerJoined(IPlayerNetworkable player)
    {
        base.OnPlayerJoined(player);

        if (NetworkManager.singleton.mode == NetworkManagerMode.Host)
        {
            player.OnColision += PlayerCollision;
        }

        if (players.Count - 1 < spawnPositions.Count)
        {
            player.GameObject.transform.position = spawnPositions[players.Count - 1].position;
            player.GameObject.transform.rotation = spawnPositions[players.Count - 1].rotation;
        }

        if (players.Count >= maxPlayers)
            Invoke(nameof(StartMatch), 2);
    }

    private void OnAsteroidDestroy(AsteroidEntity asteroid, SpaceshipEntity destroyer)
    {
        if (projectSettings == null)
            return;

        AsteroidSO asteroidData = projectSettings.projectObjects.Find(projectObject => projectObject.objectId == asteroid.Id) as AsteroidSO;
        PlayerScore score = destroyer.GetComponentInChildren<PlayerScore>();

        score?.AddScore((int)asteroidData.scoreToAdd);
    }

    private void PlayerCollision(IPlayerNetworkable player, GameObject objectCollision)
    {
        PlayerLives lives = player.GameObject.GetComponentInChildren<PlayerLives>();

        lives.RemoveLive(1);

        if (lives.Lives <= 0)
        {
            deadPlayersCount++;

            if (deadPlayersCount >= players.Count)
            {
            }
        }
        else
        {
            StartCoroutine(Respawn(player));
        }
    }

    private void StartMatch()
    {
        if (NetworkManager.singleton.mode == NetworkManagerMode.Host)
        {
            if (gameDataChannel != null)
                gameDataChannel.OnAsteroidDestroyed += OnAsteroidDestroy;
            else
                Debug.LogWarning("You are trying to Subscribte from GameDataChannel but it's null..");
        }

        if (!asteroidSpawner)
        {
            asteroidSpawner = AsteroidSpawner.Instance;
        }
        if (!enemySpawner)
        {
            enemySpawner = EnemySpawner.Instance;
        }

        gameplayView?.StartMatch(null);

        if (NetworkManager.singleton.mode == NetworkManagerMode.Host)
        {
            asteroidSpawner.Init();
            enemySpawner.Init();
            Invoke(nameof(SpawnUfo), 2);
        }

        players.ForEach(player => player?.Init());
    }

    private void SpawnUfo()
    {
        //GenericFactory.Instance.Create();
    }

    private void OnPlayerCrash(GameObject playerGO)
    {
        Player player = playerGO.GetComponent<Player>();


        player.RemoveLive(1);

        if (player.Lives <= 0)
        {
            /*if (settingChannel.gameType == GameType.Solo)
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
                        /*if (player.Score >= scoreMax)
                        {
                            scoreMax = player.Score;
                            winnerNickname = player.Nickname;

                        }*/
            // });
            //players.ForEach(player => player.FinishMatch(winnerNickname, scoreMax.ToString("0000000")));
            // }
            // }
        }
        else
        {
            // StartCoroutine(Respawn(player));
        }

        player.HidePlayer();
    }

    private IEnumerator Respawn(IPlayerNetworkable player)
    {
        yield return new WaitForSeconds(respawnDelay);

        Vector2 positionToRespawn = Vector3.zero;

        player.GameObject.transform.position = positionToRespawn;

        player.Respawn(positionToRespawn);
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
        try { gameDataChannel.OnAsteroidDestroyed -= OnAsteroidDestroy; } catch { }
        try { players.ForEach(player => player.OnColision -= PlayerCollision); } catch { }
    }
}
