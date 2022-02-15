using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmunityBehaviour : NetworkBehaviour, IImmuneAble
{
    public Action OnStopImmune { get; set; }
    public Action<float> OnStartImmune { get; set; }

    public GameObject Gameobject =>gameObject;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private float timeImmune;
    [Server]
    public void Init(float _timeImmune)
    {
        timeImmune = _timeImmune;
        ShowImmune();
    }
    [Server]
    public void Immunity(float _timeImmune)
    {
        OnStartImmune?.Invoke(_timeImmune);
    }
    [ClientRpc]
    private void ShowImmune()
    {
        spriteRenderer.color = Color.white;
    }
    [ClientRpc]
    private void HideImmune()
    {
        spriteRenderer.color = new Color(0, 0, 0, 0);
    }
    [Server]
    public void UpdateBehaviour(float dt)
    {
        timeImmune -= dt;
        if (timeImmune <= 0)
        {
            OnStopImmune?.Invoke();
            HideImmune();
        }
    }
}
