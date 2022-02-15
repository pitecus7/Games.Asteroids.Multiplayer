using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsSpawner : NetworkBehaviour
{
    public static PowerUpsSpawner Instance;
    [SerializeField] private GenericFactory factory;

    [SerializeField] private ProjectSettingsSO projectSettings;

    [SerializeField] private List<string> powerUpsListId;

    [SerializeField] private InputsReader inputsReader;

    [SerializeField] private float cheetCooldown = 10;

    private float currentCheet;

    private void Awake()
    {
        Instance = this;

        inputsReader.PowerUpCheat += CreatePowerUp;

        if (projectSettings != null)
            projectSettings.projectObjects.ForEach(projectObject =>
        {
            if (projectObject != null)
            {
                if (projectObject.GetType() == typeof(PowerUpSO))
                {
                    powerUpsListId.Add(projectObject.objectId);
                }
            }
        });

        if (factory == null)
        {
            factory = GenericFactory.Instance;
        }
    }

    private void Update()
    {
        currentCheet += Time.deltaTime;
    }

    private void CreatePowerUp()
    {
        if (NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly)
        {
            return;
        }

        if (currentCheet < cheetCooldown)
            return;

        if (powerUpsListId.Count > 0)
        {
            // Choose a random direction from the center of the spawner and
            // spawn the asteroid a distance away
            Vector2 spawnDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = spawnDirection * 3;

            // Offset the spawn point by the position of the spawner so its
            // relative to the spawner location
            spawnPoint += transform.position;

            // Calculate a random variance in the asteroid's rotation which will
            // cause its trajectory to change
            float variance = Random.Range(-5, 5);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            PowerUp powerUp = factory.Create<PowerUp>(powerUpsListId[Random.Range(0, powerUpsListId.Count)]);

            powerUp.gameObject.transform.rotation = rotation;

            powerUp?.Init(spawnPoint);
        }

        currentCheet = 0;
    }

    public void SpawnRandom(Vector2 position)
    {
        if (powerUpsListId.Count > 0)
        {
            PowerUp powerUp = factory.Create<PowerUp>(powerUpsListId[Random.Range(0, powerUpsListId.Count)]);

            powerUp?.Init(position);
        }
    }

    private void OnDestroy()
    {
        inputsReader.PowerUpCheat -= CreatePowerUp;
    }
}
