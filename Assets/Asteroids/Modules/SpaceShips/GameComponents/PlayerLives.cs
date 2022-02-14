using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLives : NetworkBehaviour
{
    public IntEvent OnLivesChange;
    public int Lives { get => lives; }

    [SyncVar(hook = nameof(LiveChanged)), SerializeField] private int lives = 3;

    private void LiveChanged(int oldValue, int newValue)
    {
        OnLivesChange?.Invoke(newValue);
    }

    [Server]
    public void AddLive(int liveToAdd)
    {
        lives += liveToAdd;
    }
    [Server]
    public void RemoveLive(int liveToRemove)
    {
        if (lives - liveToRemove >= 0)
        {
            lives -= liveToRemove;
        }
        else
        {
            lives = 0;
        }
    }
}
