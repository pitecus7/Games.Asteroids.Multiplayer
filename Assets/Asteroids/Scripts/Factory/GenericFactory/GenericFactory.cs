using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericFactory : MonoBehaviour
{
    public static GenericFactory Instance;

    [SerializeField] private GenericObjectPool objectPool;

    [SerializeField] private ProjectSettingsSO projectSettings;

    private Dictionary<string, ISpawneable> idToSpawneableObjects = new Dictionary<string, ISpawneable>();

    private List<ISpawneable> ObjectsSpawned = new List<ISpawneable>();

    void Awake()
    {
        //Singleton
        if (Instance == null)
            Instance = this;

        if (objectPool == null)
            objectPool = GetComponent<GenericObjectPool>();

        idToSpawneableObjects = new Dictionary<string, ISpawneable>();

        projectSettings.projectObjects.ForEach(spawnObject =>
        {
            ISpawneable spawneable;
            if (spawnObject != null && spawnObject.prefab && spawnObject.prefab.TryGetComponent(out spawneable))
            {
                idToSpawneableObjects.Add(spawneable.Id, spawneable);
            }
        });
    }

    public T Create<T>(string id) where T : Object
    {
        GameObject createObject = null;
        createObject = objectPool.GetObject(id);

        if (createObject == null)
        {
            if (idToSpawneableObjects.ContainsKey(id))
            {
                createObject = Instantiate(idToSpawneableObjects[id].GameObject);
                ObjectsSpawned.Add(createObject.GetComponent<ISpawneable>());
                ObjectsSpawned[ObjectsSpawned.Count - 1].OnAddToPool += AddToPool;
                //Using cuz Mirror needs
#if MIRROR
                NetworkServer.Spawn(createObject);
#endif
            }
        }
        if (createObject.TryGetComponent(out T dataResponse))
        {
            return dataResponse;
        }
        else
        {
            return null;
        }
    }

    private void AddToPool(string id, GameObject objectToAdd)
    {
        objectPool.AddObject(id, objectToAdd);
    }

    private void OnDestroy()
    {
        ObjectsSpawned.ForEach(objectSpawned =>
        {
            objectSpawned.OnAddToPool -= AddToPool;
        });
    }
}
