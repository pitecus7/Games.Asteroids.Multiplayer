using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsPool : MonoBehaviour
{
    public static BulletsPool Instance;

    [SerializeField] ClasicBullet bulletPrefab;

    Queue<ClasicBullet> bulletsPool = new Queue<ClasicBullet>();

    private void Awake()
    {
        Instance = this;
    }

    public ClasicBullet GetPooledObject(Vector2 position, Quaternion rotation)
    {
        if (bulletsPool.Count > 0)
        {
            ClasicBullet bullet = bulletsPool.Dequeue();
            bullet.ShowToClients(position, rotation);
            bullet.gameObject.transform.position = position;
            bullet.gameObject.transform.rotation = rotation;
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        else
        {
            ClasicBullet asteroid = Instantiate(bulletPrefab, position, rotation);
            NetworkServer.Spawn(asteroid.gameObject);
            return asteroid;
        }
    }

    public void AddToPool(ClasicBullet bullet)
    {
        bullet.HideToClients();
        bullet.gameObject.SetActive(false);
        bulletsPool.Enqueue(bullet);
    }
}
