using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsPool : MonoBehaviour
{
    public static BulletsPool Instance;

    [SerializeField] Bullet bulletPrefab;

    Queue<Bullet> bulletsPool = new Queue<Bullet>();

    private void Awake()
    {
        Instance = this;
    }

    public Bullet GetPooledObject(Vector2 position, Quaternion rotation)
    {
       /* if (bulletsPool.Count > 0)
        {
            Bullet bullet = bulletsPool.Dequeue();
            bullet.ShowToClients(position);
            bullet.gameObject.transform.position = position;
            bullet.gameObject.transform.rotation = rotation;
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        else
        {*/
            Bullet asteroid = Instantiate(bulletPrefab, position, rotation);
            NetworkServer.Spawn(asteroid.gameObject);
            return asteroid;
        //}
    }

    public void AddToPool(Bullet bullet)
    {
        Destroy(bullet.gameObject);
        //bullet.HideToClients();
        //bullet.gameObject.SetActive(false);
        //bulletsPool.Enqueue(bullet);
    }
}
