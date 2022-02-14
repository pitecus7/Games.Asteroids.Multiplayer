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

    private void Awake()
    {
        Instance = this;

        if (projectSettings != null)
            projectSettings.projectObjects.ForEach(projectObject =>
        {
            if (projectObject.GetType() == typeof(PowerUpSO))
            {
                powerUpsListId.Add(projectObject.objectId);
            }
        });

        if (factory == null)
        {
            factory = GenericFactory.Instance;
        }
    }

    public void SpawnRandom(Vector2 position)
    {
        if (powerUpsListId.Count > 0)
        {
            PowerUp powerUp = factory.Create<PowerUp>(powerUpsListId[Random.Range(0, powerUpsListId.Count)]);

            powerUp?.Init(position);
        }
    }
}
