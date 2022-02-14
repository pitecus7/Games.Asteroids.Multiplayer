using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayersChannel", menuName = "ScriptableObjects/PlayersChannel", order = 1)]
public class PlayersChannel : ScriptableObject
{
    public Action<IPlayerNetworkable> OnPlayerJoined;

    public void AddPlayer(GameObject player)
    {
        if (player.TryGetComponent(out IPlayerNetworkable playerNetwork))
        {
            OnPlayerJoined?.Invoke(playerNetwork);
        }
        else
        {
            Debug.LogWarning("Trying to Add a player and it's not a player.");
        }
    }
}
