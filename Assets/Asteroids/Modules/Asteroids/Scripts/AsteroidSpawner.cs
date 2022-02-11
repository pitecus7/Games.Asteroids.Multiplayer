using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : NetworkBehaviour
{
    public static AsteroidSpawner Instance;
    public Asteroid asteroidPrefab;
    public float spawnDistance = 12f;
    public float spawnRate = 3f;
    public int amountPerSpawn = 1;

    public float timeToAddDificulty = 30;//Seconds

    [Range(0f, 45f)]
    public float trajectoryVariance = 15f;

    Queue<Asteroid> asteroidsPool = new Queue<Asteroid>();

    [SerializeField] private int difficulty = 0;

    private void Awake()
    {
        Instance = this;
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
            Asteroid asteroid = GetPooledObject(spawnPoint, rotation);
            

            asteroid.SetValues();
            // Set the trajectory to move in the direction of the spawner
            Vector2 trajectory = rotation * -spawnDirection;
            asteroid.SetTrajectory(trajectory);
        }
    }
    [ServerCallback]
    public void SpawnNewAsteriods(float currentSize, Vector2 position, Quaternion rotation)
    {

        // Create the new asteroid at half the size of the current
        Asteroid half = GetPooledObject(position, rotation);

        // Set a random trajectory
        half.SetTrajectory(Random.insideUnitCircle.normalized);

        half.SetValues(currentSize * 0.5f);
    }

    public Asteroid GetPooledObject(Vector2 position, Quaternion rotation)
    {
        if (asteroidsPool.Count > 0)
        {
            Asteroid asteroid = asteroidsPool.Dequeue();
            asteroid.ShowToClients(position);
            asteroid.gameObject.transform.position = position;
            asteroid.gameObject.transform.rotation = rotation;
            asteroid.gameObject.SetActive(true);
            return asteroid;
        }
        else
        {
            Asteroid asteroid = Instantiate(asteroidPrefab, position, rotation);
            NetworkServer.Spawn(asteroid.gameObject);
            return asteroid;
        }
    }

    public void AddToPool(Asteroid asteroid)
    {
        asteroid.HideToClients();
        asteroid.gameObject.SetActive(false);
        asteroidsPool.Enqueue(asteroid);
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}
