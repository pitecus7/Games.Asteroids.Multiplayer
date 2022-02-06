using Mirror;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : NetworkBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private float speed = 500f;
    [SerializeField] private float maxLifetime = 5f;

    private Player playerShooter;
    public Player PlayerShooter => playerShooter;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction, Player player)
    {
        playerShooter = player;
        playerShooter.RemoveScore(10);
        // The bullet only needs a force to be added once since they have no
        // drag to make them stop moving
        rigidbody.AddForce(direction * speed);

        // Destroy the bullet after it reaches it max lifetime
        Invoke("HideBullet", maxLifetime);
    }

    private void HideBullet()
    {
        CancelInvoke();
        BulletsPool.Instance.AddToPool(this);
    }

    [ClientRpc]
    public void HideToClients()
    {
        this.gameObject.SetActive(false);
    }

    [ClientRpc]
    public void ShowToClients(Vector2 position)
    {
        this.gameObject.transform.position = position;
        //this.gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy the bullet as soon as it collides with anything
        BulletsPool.Instance.AddToPool(this);
    }

}
