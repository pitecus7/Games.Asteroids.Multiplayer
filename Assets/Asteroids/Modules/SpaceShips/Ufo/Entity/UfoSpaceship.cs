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

    [SerializeField] private SpaceshipEntity target;
    public SpaceshipEntity Target => target;

    [SerializeField] private List<SpaceshipEntity> targets;

    bool targetLastState;

    [SerializeField]
    private void OnEnable()
    {
        SeletTarget();
    }

    private void SeletTarget()
    {
        targets.AddRange(FindObjectsOfType<PlayerSpaceship>());

        targets = targets.FindAll(_target => _target.isInactive == false);

        int randomTarget = Random.Range(0, targets.Count);

        if (targets.Count > 0)
            target = targets[randomTarget];

        targetLastState = target.isInactive;
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
    [Server]
    public override void Init()
    {
        ShowToClients(transform.position);
    }

    [Server]
    private void FixedUpdate()
    {
        if (targetLastState != target.isInactive)
        {
            SeletTarget();
        }
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

    [ClientRpc]
    public void ShowToClients(Vector2 position)
    {
        gameObject.transform.position = position;
        gameObject.SetActive(true);
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
