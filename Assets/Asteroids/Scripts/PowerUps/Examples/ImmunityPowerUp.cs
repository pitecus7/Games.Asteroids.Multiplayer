using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmunityPowerUp : PowerUp
{
    [SerializeField] private string id;
    public override string Id => id;

    [SerializeField] private PowerUpSO powerUpData;

    [Server]
    public override void Init(Vector2 position)
    {
        transform.position = position;
        ShowToClients(position);
    }

    [Server]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IImmuneAble immuneCompoenent = collision.gameObject.GetComponentInChildren<IImmuneAble>();
        immuneCompoenent?.Immunity(powerUpData.duration);
        HideToClients();
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