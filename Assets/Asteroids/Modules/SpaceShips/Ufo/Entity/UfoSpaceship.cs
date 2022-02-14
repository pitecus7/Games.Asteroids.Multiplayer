using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoSpaceship : SpaceshipEntity
{
    [SerializeField]
    private IFlyAble spaceshipMovement;
    [SerializeField]
    private IShootAble spaceshipShoot;

    [SerializeField] private GameObject targetGameObject;
    public Transform Target => targetGameObject.transform;

    [SerializeField] private List<SpaceshipEntity> targets;

    [SerializeField]
    private void OnEnable()
    {
        SeletTarget();
    }

    private void SeletTarget()
    {
        targets.AddRange(FindObjectsOfType<PlayerSpaceship>());
        int randomTarget = Random.Range(0, targets.Count);
        targetGameObject = targets[randomTarget].GameObject;
    }

    private void Awake()
    {
        if (spaceshipMovement == null)
        {
            spaceshipMovement = GetComponentInChildren<IFlyAble>();
        }
        if (spaceshipShoot == null)
        {
            spaceshipShoot = GetComponentInChildren<IShootAble>();
        }
    }

    public override void Init()
    {
    }

    [Server]
    private void FixedUpdate()
    {
        spaceshipMovement?.UpdateBehaviour(Time.deltaTime);
        spaceshipShoot?.UpdateBehaviour(Time.deltaTime);
    }

    [ClientRpc]
    public void HideSpace()
    {
        gameObject.SetActive(false);
    }

    [ClientRpc]
    public void ShowExplosive()
    {
        VfxController.Instance?.Explote(transform.position);
    }

    [Server]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HideSpace();
        ShowExplosive();

        SpaceshipEntity shooter;
        BulletEntity bullet;

        if (collision.gameObject.TryGetComponent(out bullet) && bullet.Shooter.TryGetComponent(out shooter))
            gameDataChannel.EnemyDestroy(this, shooter);    
        else if (collision.gameObject.TryGetComponent(out shooter))
            gameDataChannel.EnemyDestroy(this, shooter);

        OnAddToPool?.Invoke(Id, gameObject);
    }
}
