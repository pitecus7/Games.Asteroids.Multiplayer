using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectPool : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

    public void AddObject(string id, GameObject objectToAdd)
    {
        if (pool.ContainsKey(id))
        {
            pool[id].Enqueue(objectToAdd);
        }
        else
        {
            pool.Add(id, new Queue<GameObject>());
            pool[id].Enqueue(objectToAdd);
        }
    }

    public GameObject GetObject(string id)
    {
        if (pool.ContainsKey(id) && pool[id].Count > 0)
        {
            return pool[id].Dequeue();
        }
        else
        {
            return null;
        }
    }
}
