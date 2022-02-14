using Mirror;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField] private GenericFactory factory;

    [SerializeField] private GameDataChannel gameDataChannel;

    [SerializeField] private ProjectSettingsSO projectSettings;

    [SerializeField] private List<string> enemyListId;

    [Range(0f, 45f)]
    [SerializeField] private float trajectoryVariance = 15f;

    [SerializeField] private float spawnDistance = 12f;

    [SerializeField] private float spawnRate = 60f;

    private void Awake()
    {
        Instance = this;

        projectSettings.projectObjects.ForEach(projectObject =>
        {
            if (projectObject.GetType() == typeof(EnemySpaceshipSO))
            {
                enemyListId.Add(projectObject.objectId);
            }
        });

        gameDataChannel.OnEnemyDestroyed += EnemyDestroyed;

        if (factory == null)
        {
            factory = GenericFactory.Instance;
        }
    }

    private void EnemyDestroyed(SpaceshipEntity arg1, SpaceshipEntity arg2)
    {

    }

    public void Spawn()
    {
        if (enemyListId.Count > 0)
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

            SpaceshipEntity enemy = factory.Create<SpaceshipEntity>(enemyListId[Random.Range(0, enemyListId.Count)]);

            enemy.gameObject.transform.position = spawnPoint;
            enemy.gameObject.transform.rotation = rotation;

            enemy.Init();
        }
    }

    public void Init()
    {
        if (isServer)
        {
            InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
        }
    }

    public void Stop()
    {
        CancelInvoke();
    }
}
