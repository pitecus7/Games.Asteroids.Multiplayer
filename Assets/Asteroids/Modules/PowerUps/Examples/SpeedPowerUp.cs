using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    [SerializeField] private string id;
    public override string Id => id;

    [SerializeField] private PowerUpSO powerUpData;

    private IFlyAble target;

    private float basicSpeed;
    private float basicSpeedRotation;

    [Server]
    public override void Init(Vector2 position)
    {
        transform.position = position;
        ShowToClients(position);
    }

    [Server]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        target = collision.gameObject.GetComponentInChildren<IFlyAble>();
        if (target != null)
        {
            basicSpeed = target.Speed;
            basicSpeedRotation = target.SpeedRotation;
            if (collision.gameObject.TryGetComponent(out SpaceshipActor actor))
            {
                actor.OnColision += OnTargetCollision;
            }
        }
        target?.SetSpeed(basicSpeed * powerUpData.increse);
        target?.SetSpeedRotation(basicSpeedRotation * powerUpData.increse);
        HideToClients();
        Invoke(nameof(RestoreTarget), powerUpData.duration);
        gameObject.transform.position = Vector2.one * 100;
    }
    [Server]
    private void OnTargetCollision(GameObject obj)
    {
        if (target.Gameobject.TryGetComponent(out SpaceshipActor actor))
        {
            actor.OnColision -= OnTargetCollision;
        }
        RestoreTarget();
    }

    [Server]
    private void RestoreTarget()
    {
        target?.SetSpeed(basicSpeed);
        target?.SetSpeedRotation(basicSpeedRotation);
        OnAddToPool?.Invoke(id, gameObject);
    }

    [ClientRpc]
    public void HideToClients()
    {
        transform.position = Vector2.one * 100;
    }

    [ClientRpc]
    public void ShowToClients(Vector2 position)
    {
        transform.position = position;
    }
}
