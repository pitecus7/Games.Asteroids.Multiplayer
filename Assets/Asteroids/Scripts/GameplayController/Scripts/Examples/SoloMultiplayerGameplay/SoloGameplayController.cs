using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloGameplayController : GameplayControllerCore
{
    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private EnemySpawner enemySpawner;

    [SerializeField] private GameDataChannel gameDataChannel;

    [SerializeField] private GameplayStatus gameplayStatus;

    [SerializeField] private ProjectSettingsSO projectSettings;

    [SerializeField] private float respawnDelay = 3f;

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
        if (!gameplayStatus)
        {
            gameplayStatus = GameplayStatus.Instance;
        }

        callback?.Invoke();
    }

    protected override void OnPlayerJoined(IPlayerNetworkable player)
    {
        base.OnPlayerJoined(player);

        player.OnColision += PlayerCollision;

        Invoke(nameof(StartMatch), 2);
    }

    private void OnAsteroidDestroy(AsteroidEntity asteroid, SpaceshipEntity destroyer)
    {
        if (projectSettings == null)
            return;

        AsteroidSO asteroidData = projectSettings.projectObjects.Find(projectObject => projectObject?.objectId == asteroid.Id) as AsteroidSO;
        PlayerScore score = destroyer.GetComponentInChildren<PlayerScore>();

        score?.AddScore((int)asteroidData.scoreToAdd);
    }

    private void PlayerCollision(IPlayerNetworkable player, GameObject objectCollision)
    {
        PlayerLives lives = player.GameObject.GetComponentInChildren<PlayerLives>();

        lives.RemoveLive(1);

        if (lives.Lives <= 0)
        {
            asteroidSpawner.Stop();
            enemySpawner.Stop();

            gameplayView?.FinishMatch();
        }
        else
        {
            StartCoroutine(Respawn(player));
        }
    }

    private void StartMatch()
    {

        if (gameDataChannel != null)
            gameDataChannel.OnAsteroidDestroyed += OnAsteroidDestroy;
        else
            Debug.LogWarning("You are trying to Subscribte from GameDataChannel but it's null..");


        if (!asteroidSpawner)
        {
            asteroidSpawner = AsteroidSpawner.Instance;
        }
        if (!enemySpawner)
        {
            enemySpawner = EnemySpawner.Instance;
        }

        gameplayView?.StartMatch(null);


        asteroidSpawner.Init();
        enemySpawner.Init();

        players.ForEach(player => player?.Init());
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
