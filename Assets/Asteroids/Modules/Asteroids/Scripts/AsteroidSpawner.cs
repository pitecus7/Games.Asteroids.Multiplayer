using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : NetworkBehaviour
{
    public static AsteroidSpawner Instance;
    [SerializeField] private GenericFactory factory;

    [SerializeField] private GameDataChannel gameDataChannel;
    [SerializeField] private ProjectSettingsSO projectSettings;

    [SerializeField] private float spawnDistance = 12f;
    [SerializeField] private float spawnRate = 3f;
    [SerializeField] private int amountPerSpawn = 1;

    [SerializeField] private float timeToAddDificulty = 30;//Seconds

    [Range(0f, 45f)]
    [SerializeField] private float trajectoryVariance = 15f;

    [SerializeField] private int difficulty = 0;

    private void Awake()
    {
        Instance = this;

        gameDataChannel.OnAsteroidDestroyed += AsteroidDestroyed;
        if (factory == null)
        {
            factory = GenericFactory.Instance;
        }
    }

    private void AsteroidDestroyed(AsteroidEntity asteroid, SpaceshipEntity destroyer)
    {
        AsteroidSO asteroidData = projectSettings.projectObjects.Find(projectObject => projectObject.objectId == asteroid.Id) as AsteroidSO;

        for (int i = 0; i < asteroidData.dividePartsNumber; i++)
        {
            Vector2 position = asteroid.transform.position;
            position += Random.insideUnitCircle * 0.5f;

            AsteroidEntity newAsteroid = factory.Create<AsteroidEntity>(asteroidData.divideObject.objectId);

            newAsteroid.gameObject.transform.position = position;

            newAsteroid.Init(Random.insideUnitCircle.normalized);
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        CancelInvoke(nameof(Spawn));
        CancelInvoke(nameof(MoreDificulty));
    }

    public void Init()
    {
        InvokeRepeating(nameof(MoreDificulty), timeToAddDificulty, timeToAddDificulty);
        if (isServer)
        {
            MoreDificulty();
        }
    }

    public void Stop()
    {
        CancelInvoke();
    }

    public void MoreDificulty()
    {
        difficulty++;
        CancelInvoke(nameof(Spawn));
        InvokeRepeating(nameof(Spawn), spawnRate / difficulty, spawnRate / difficulty);
    }

    public void Spawn()
    {
        for (int i = 0; i < amountPerSpawn; i++)
        {
            // Choose a random direction from the center of the spawner and
            // spawn the asteroid a distance away
            Vector2 spawnDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = spawnDirection * spawnDistance;

            // Offset the spawn point by the position of the spawner so its
            // relative to the spawner location
            spawnPoint += transform.position;

            // Calculate a random variance in the asteroid's rotation which will
            // cause its trajectory to change
            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            // Create the new asteroid by cloning the prefab and set a random
            // size within the range
            AsteroidEntity asteroid = factory.Create<AsteroidEntity>("BigAsteroid");
            asteroid.gameObject.transform.position = spawnPoint;
            asteroid.gameObject.transform.rotation = rotation;

            Vector2 trajectory = rotation * -spawnDirection;
            asteroid.Init(trajectory);
        }
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}
