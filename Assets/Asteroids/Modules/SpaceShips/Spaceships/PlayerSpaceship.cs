using Mirror;
using System;
using UnityEngine;

public class PlayerSpaceship : SpaceshipEntity, IPlayerNetworkable
{
    private int playerId;

    [SerializeField]
    private bool isImmune;
    [SerializeField]
    private IFlyAble spaceshipMovement;
    [SerializeField]
    private IShootAble spaceshipShoot;
    [SerializeField]
    private IImmuneAble spaceshipImmune;
    [SerializeField]
    private PlayersChannel playersChannel;

    [SerializeField]
    private PlayerNickname playerNickname;

    public int PlayerId => playerId;

    public Action<IPlayerNetworkable, GameObject> OnColision { get; set; }

    public bool IsLocal => isLocalPlayer;

    public string Nickname => playerNickname.Nickname;

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
        if (spaceshipImmune == null)
        {
            spaceshipImmune = GetComponentInChildren<IImmuneAble>();
            spaceshipImmune.OnStopImmune += StopImmunity;
            spaceshipImmune.OnStartImmune += Immunity;
        }
        if (playerNickname == null)
        {
            playerNickname = GetComponentInChildren<PlayerNickname>();
        }
    }

    private void StopImmunity()
    {
        isImmune = false;
    }

    private void Start()
    {
        playersChannel?.AddPlayer(gameObject);
    }

    [Server]
    private void Immunity(float timeImmune)
    {
        spaceshipImmune?.Init(timeImmune);
        isImmune = true;
    }

    public override void Init()
    {
        isInactive = false;
        playerNickname?.HideNickname();
    }

    private void FixedUpdate()
    {
        if (isImmune)
            spaceshipImmune?.UpdateBehaviour(Time.deltaTime);

        if (!isLocalPlayer)
            return;

        spaceshipMovement?.UpdateBehaviour(Time.deltaTime);
        spaceshipShoot?.UpdateBehaviour(Time.deltaTime);
    }

    [ClientRpc]
    public void HidePlayer()
    {
        gameObject.SetActive(false);
    }

    [ClientRpc]
    public void Respawn(Vector2 position)
    {
        isInactive = false;
        spaceshipImmune.Immunity(3);
        transform.position = position;
        gameObject.SetActive(true);
    }

    [ClientRpc]
    public void ShowExplosive()
    {
        VfxController.Instance?.Explote(transform.position);
    }

    [Server]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isImmune)
        {
            HidePlayer();
            ShowExplosive();
            isInactive = true;
            OnColision?.Invoke(this, collision.gameObject);
        }
    }
}
